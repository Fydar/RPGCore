using Newtonsoft.Json;
using RPGCore.Demo.BoardGame.Models;
using RPGCore.Packages.Pipeline;
using System.Collections.Generic;
using System.Linq;

namespace RPGCore.Demo.BoardGame
{
	/// <summary>
	/// Tags appropriate resources using it's directory.
	/// </summary>
	public class BoardGameResourceImporter : ImportProcessor
	{
		public override void ProcessImport(ProjectResourceImporter importer)
		{
			if (importer.FileInfo.FullName.Contains("buildings"))
			{
				importer.ImporterTags.Add("type-building");

				var loaded = Load<BuildingTemplate>(importer);
				importer.Dependencies.Register(loaded.PackIdentifier);

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
							importer.Dependencies.Register(resource);
							alreadyRegistered.Add(resource);
						}
					}
				}
			}
			else if (importer.FileInfo.FullName.Contains("resources"))
			{
				importer.ImporterTags.Add("type-resource");
			}
			else if (importer.FileInfo.FullName.Contains("building-packs"))
			{
				importer.ImporterTags.Add("type-buildingpack");
			}
			else if (importer.FileInfo.FullName.Contains("gamerules"))
			{
				importer.ImporterTags.Add("type-gamerules");

				var loaded = Load<GameRulesTemplate>(importer);

				var alreadyRegistered = new List<string>();
				foreach (string resource in loaded.SharedCards.Concat(loaded.PlayerCards))
				{
					if (string.IsNullOrEmpty(resource))
					{
						continue;
					}

					if (!alreadyRegistered.Contains(resource))
					{
						importer.Dependencies.Register(resource);
						alreadyRegistered.Add(resource);
					}
				}
			}
		}

		private static TModel Load<TModel>(ProjectResourceImporter importer)
			where TModel : IResourceModel
		{
			var serializer = new JsonSerializer();
			using var file = importer.FileInfo.OpenText();
			using var reader = new JsonTextReader(file);

			var model = serializer.Deserialize<TModel>(reader);
			model.Identifier = importer.ProjectKey;
			return model;
		}
	}
}
