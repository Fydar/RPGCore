using Newtonsoft.Json;
using RPGCore.Packages;
using RPGCore.Projects.Pipeline;
using System;
using System.Collections.Generic;
using System.IO;

namespace RPGCore.Projects.Extensions.MetaFiles
{
	public class JsonMetaFileImportProcessor : IImportProcessor
	{
		private readonly JsonMetaFilesOptions options;

		public JsonMetaFileImportProcessor(JsonMetaFilesOptions options)
		{
			this.options = options;
		}

		public bool CanProcess(IResource resource)
		{
			return true;
		}

		public IEnumerable<ProjectResourceUpdate> ProcessImport(ImportProcessorContext context, IResource resource)
		{
			string metaPath = $"{resource.FullName}{options.MetaFileSuffix}";
			var metaFile = new FileInfo(metaPath);

			if (metaFile.Exists)
			{
				var update = resource.AuthorUpdate();

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
						update.ImporterTags.Add(tag);
					}
				}

				yield return update;
			}
			else if (!options.IsMetaFilesOptional)
			{
				Console.WriteLine($"Missing meta file for {resource.FullName}");
			}
		}
	}
}
