using RPGCore.Packages.Pipeline;
using System.Collections.Generic;
using System.IO;

namespace RPGCore.Packages
{
	public class ImportPipeline
	{
		private List<ImportProcessor> importProcessors { get; }

		internal ImportPipeline(List<ImportProcessor> importProcessors)
		{
			this.importProcessors = importProcessors;
		}

		public ProjectResource ImportResource(ProjectExplorer projectExplorer, FileInfo fileInfo, string projectKey)
		{
			var resourceImporter = new ProjectResourceImporter(projectExplorer, fileInfo, projectKey);

			foreach (var importer in importProcessors)
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
