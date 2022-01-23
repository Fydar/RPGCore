using RPGCore.Projects.Pipeline;
using System.Collections.Generic;

namespace RPGCore.Projects;

internal sealed class ImportPipelineBuilder : IImportPipelineBuilder
{
	internal List<IArchiveDirectoryImporter> DirectoryImporters { get; }
	internal List<IArchiveFileImporter> FileImporters { get; }
	internal List<IImportProcessor> ImportProcessors { get; }

	IReadOnlyList<IArchiveDirectoryImporter> IImportPipelineBuilder.DirectoryImporters => DirectoryImporters;
	IReadOnlyList<IArchiveFileImporter> IImportPipelineBuilder.FileImporters => FileImporters;
	IReadOnlyList<IImportProcessor> IImportPipelineBuilder.ImportProcessors => ImportProcessors;

	internal ImportPipelineBuilder()
	{
		DirectoryImporters = new List<IArchiveDirectoryImporter>()
		{
			new DefaultArchiveDirectoryImporter()
		};
		FileImporters = new List<IArchiveFileImporter>()
		{
			new DefaultArchiveFileImporter()
		};
		ImportProcessors = new List<IImportProcessor>();
	}

	public ImportPipeline Build()
	{
		return new ImportPipeline(this);
	}

	public IImportPipelineBuilder UseImporter(IArchiveDirectoryImporter archiveDirectoryImporter)
	{
		DirectoryImporters.Add(archiveDirectoryImporter);
		return this;
	}

	public IImportPipelineBuilder UseImporter(IArchiveFileImporter archiveFileImporter)
	{
		FileImporters.Add(archiveFileImporter);
		return this;
	}

	public IImportPipelineBuilder UseProcessor(IImportProcessor importProcessor)
	{
		ImportProcessors.Add(importProcessor);
		return this;
	}
}
