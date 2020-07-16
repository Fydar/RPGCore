using Newtonsoft.Json;
using RPGCore.Packages.Pipeline;
using System;
using System.IO;

namespace RPGCore.Packages.Extensions.MetaFiles
{
	public class JsonMetaFileImportProcessor : ImportProcessor
	{
		private readonly JsonMetaFilesOptions options;

		public JsonMetaFileImportProcessor(JsonMetaFilesOptions options)
		{
			this.options = options;
		}

		public override void ProcessImport(ProjectResourceImporter projectResource)
		{
			string metaPath = $"{projectResource.ArchiveEntry.FullName}{options.MetaFileSuffix}";
			var metaFile = new FileInfo(metaPath);

			if (metaFile.Exists)
			{
				JsonMetaFileModel metaFileModel;

				var serializer = new JsonSerializer();
				using (var file = metaFile.OpenText())
				using (var reader = new JsonTextReader(file))
				{
					metaFileModel = serializer.Deserialize<JsonMetaFileModel>(reader);
				}

				if (metaFileModel.Tags != null)
				{
					foreach (string tag in metaFileModel.Tags)
					{
						projectResource.ImporterTags.Add(tag);
					}
				}
			}
			else if (!options.IsMetaFilesOptional)
			{
				Console.WriteLine($"Missing meta file for {projectResource.ArchiveEntry.FullName}");
			}
		}
	}
}
