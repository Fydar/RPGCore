using RPGCore.FileTree;
using RPGCore.Projects.Pipeline;
using System.Collections.Generic;

namespace RPGCore.Projects;

/// <summary>
/// <para>A configurable and extendable pipeline for the importation of assets.</para>
/// </summary>
public class ImportPipeline
{
	private readonly List<IArchiveDirectoryImporter> directoryImporters;
	private readonly List<IArchiveFileImporter> fileImporters;
	internal readonly List<IImportProcessor> importProcessors;

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

	/// <summary>
	/// <para>Begins the construction of a <see cref="ImportPipeline"/> via a <see cref="IImportPipelineBuilder"/>.</para>
	/// </summary>
	/// <returns>A builder that can be used to add features to the import pipeline.</returns>
	public static IImportPipelineBuilder Create()
	{
		return new ImportPipelineBuilder();
	}
}
