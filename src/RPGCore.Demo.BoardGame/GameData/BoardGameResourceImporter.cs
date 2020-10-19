using Newtonsoft.Json;
using RPGCore.Demo.BoardGame.Models;
using RPGCore.Packages;
using RPGCore.Packages.Archives;
using RPGCore.Packages.Pipeline;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RPGCore.Demo.BoardGame
{
	/// <summary>
	/// Tags appropriate resources using it's directory.
	/// </summary>
	public class BoardGameResourceImporter : IArchiveFileImporter
	{
		public bool CanImport(IArchiveFile archiveFile)
		{
			return archiveFile.Extension == ".json";
		}

		public IEnumerable<ProjectResourceUpdate> ImportFile(ArchiveFileImporterContext context, IArchiveFile archiveFile)
		{
			var update = context.AuthorUpdate(archiveFile.FullName)
				.WithContent(archiveFile);

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
			}
			else if (archiveFile.FullName.Contains("resources"))
			{
				update.ImporterTags.Add("type-resource");
			}
			else if (archiveFile.FullName.Contains("building-packs"))
			{
				update.ImporterTags.Add("type-buildingpack");
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
			}

			yield return update;
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
}
