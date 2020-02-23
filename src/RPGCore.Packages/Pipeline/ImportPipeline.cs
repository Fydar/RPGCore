using RPGCore.Packages.Pipeline;
using System.Collections.Generic;
using System.IO;

namespace RPGCore.Packages
{
	public class ImportPipeline
	{
		public List<ProjectResourceImportProcessor> ImportProcessors { get; }

		public ImportPipeline()
		{
			ImportProcessors = new List<ProjectResourceImportProcessor>();
		}

		public ProjectResource ImportResource(ProjectExplorer projectExplorer, FileInfo fileInfo, string projectKey)
		{
			var resourceImporter = new ProjectResourceImporter(projectExplorer, fileInfo, projectKey);

			foreach (var importer in ImportProcessors)
			{
				importer.ProcessImport(resourceImporter);
			}

			return resourceImporter.Import();
		}
	}
}
