using RPGCore.FileTree;
using RPGCore.Projects.Pipeline;
using System.Collections.Generic;

namespace RPGCore.Projects
{
	public class ImportPipeline
	{
		private List<IArchiveDirectoryImporter> directoryImporters { get; }
		private List<IArchiveFileImporter> fileImporters { get; }
		internal List<IImportProcessor> importProcessors { get; }

		internal ImportPipeline(ImportPipelineBuilder builder)
		{
			directoryImporters = builder.DirectoryImporters;
			fileImporters = builder.FileImporters;
			importProcessors = builder.ImportProcessors;
		}

		internal IEnumerable<ProjectResourceUpdate> ImportDirectory(ProjectExplorer projectExplorer, IArchiveDirectory archiveDirectory)
		{
			for (int i = directoryImporters.Count - 1; i >= 0; i--)
			{
				var directoryImporter = directoryImporters[i];

				if (directoryImporter.CanImport(archiveDirectory))
				{
					foreach (var update in directoryImporter.ImportDirectory(
						new ArchiveDirectoryImporterContext()
						{
							Explorer = projectExplorer,
							Source = archiveDirectory
						}, archiveDirectory))
					{
						yield return update;
					}
					break;
				}
			}
		}

		internal IEnumerable<ProjectResourceUpdate> ImportFile(ProjectExplorer projectExplorer, IArchiveFile archiveFile)
		{
			for (int i = fileImporters.Count - 1; i >= 0; i--)
			{
				var fileImporter = fileImporters[i];

				if (fileImporter.CanImport(archiveFile))
				{
					foreach (var update in fileImporter.ImportFile(
						new ArchiveFileImporterContext()
						{
							Explorer = projectExplorer,
							Source = archiveFile
						}, archiveFile))
					{
						yield return update;
					}
					break;
				}
			}
		}

		public static IImportPipelineBuilder Create()
		{
			return new ImportPipelineBuilder();
		}
	}
}
