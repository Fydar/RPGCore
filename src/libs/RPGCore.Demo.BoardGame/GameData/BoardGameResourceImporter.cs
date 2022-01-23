using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGCore.Demo.BoardGame.Models;
using RPGCore.FileTree;
using RPGCore.Projects;
using RPGCore.Projects.Pipeline;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RPGCore.Demo.BoardGame;

/// <summary>
/// Tags appropriate resources using it's directory.
/// </summary>
public class BoardGameResourceImporter : IArchiveFileImporter
{
	private class JsonContentWriter : IContentWriter
	{
		private readonly object content;

		public JsonContentWriter(object content)
		{
			this.content = content;
		}

		public Task WriteContentAsync(Stream destination)
		{
			var serializer = new JsonSerializer();
			using var streamWriter = new StreamWriter(destination);
			serializer.Serialize(streamWriter, content);
			return Task.CompletedTask;
		}
	}

	/// <inheritdoc/>
	public bool CanImport(IArchiveFile archiveFile)
	{
		return archiveFile.Extension == ".json";
	}

	/// <inheritdoc/>
	public IEnumerable<ProjectResourceUpdate> ImportFile(ArchiveFileImporterContext context, IArchiveFile archiveFile)
	{
		var update = context.AuthorUpdate(archiveFile.FullName);
		object content;

		if (archiveFile.FullName.Contains("buildings"))
		{
			update.ImporterTags.Add("type-building");

			var loaded = Load<BuildingTemplate>(archiveFile.FullName, archiveFile);
			update.Dependencies.Register(loaded.PackIdentifier);

			if (loaded.Recipe != null)
			{
				var alreadyRegistered = new List<string>();
				foreach (string resource in loaded.Recipe)
				{
					if (string.IsNullOrEmpty(resource))
					{
						continue;
					}

					if (!alreadyRegistered.Contains(resource))
					{
						update.Dependencies.Register(resource);
						alreadyRegistered.Add(resource);
					}
				}
			}

			content = loaded;
		}
		else if (archiveFile.FullName.Contains("resources"))
		{
			update.ImporterTags.Add("type-resource");

			content = LoadJObject(archiveFile);
		}
		else if (archiveFile.FullName.Contains("building-packs"))
		{
			update.ImporterTags.Add("type-buildingpack");

			content = LoadJObject(archiveFile);
		}
		else if (archiveFile.FullName.Contains("gamerules"))
		{
			update.ImporterTags.Add("type-gamerules");

			var loaded = Load<GameRulesTemplate>(archiveFile.FullName, archiveFile);

			var alreadyRegistered = new List<string>();
			foreach (string resource in loaded.SharedCards.Concat(loaded.PlayerCards))
			{
				if (string.IsNullOrEmpty(resource))
				{
					continue;
				}

				if (!alreadyRegistered.Contains(resource))
				{
					update.Dependencies.Register(resource);
					alreadyRegistered.Add(resource);
				}
			}

			content = loaded;
		}
		else
		{
			content = LoadJObject(archiveFile);
		}

		var jsonContentWriter = new JsonContentWriter(content);
		update.WithContent(jsonContentWriter);

		yield return update;
	}

	private static object LoadJObject(IArchiveFile importer)
	{
		var serializer = new JsonSerializer();
		using var file = importer.OpenRead();
		using var sr = new StreamReader(file);
		using var reader = new JsonTextReader(sr);

		var model = serializer.Deserialize<JObject>(reader);
		return model;
	}

	private static TModel Load<TModel>(string identifier, IArchiveFile importer)
		where TModel : IResourceModel
	{
		var serializer = new JsonSerializer();
		using var file = importer.OpenRead();
		using var sr = new StreamReader(file);
		using var reader = new JsonTextReader(sr);

		var model = serializer.Deserialize<TModel>(reader);
		model.Identifier = identifier;
		return model;
	}
}
