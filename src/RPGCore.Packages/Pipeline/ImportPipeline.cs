using RPGCore.Packages.Pipeline;
using System.Collections.Generic;
using System.IO;

namespace RPGCore.Packages
{
	public class ImportPipeline
	{
		private List<ImportProcessor> processors { get; }
		private List<ImportFilter> filters { get; }

		internal ImportPipeline(ImportPipelineBuilder builder)
		{
			processors = builder.Processors;
			filters = builder.Filters;
		}

		public bool IsResource(FileInfo fileInfo)
		{
			foreach (var filter in filters)
			{
				if (!filter.AllowFile(fileInfo))
				{
					return false;
				}
			}
			return true;
		}

		public ProjectResource ImportResource(ProjectExplorer projectExplorer, FileInfo fileInfo, string projectKey)
		{
			var resourceImporter = new ProjectResourceImporter(projectExplorer, fileInfo, projectKey);

			foreach (var importer in processors)
			{
				importer.ProcessImport(resourceImporter);
			}

			return resourceImporter.Import();
		}

		public static IImportPipelineBuilder Create()
		{
			return new ImportPipelineBuilder();
		}
	}
}
