using RPGCore.Projects.Pipeline;
using System.Collections.Generic;

namespace RPGCore.Projects
{
	public interface IImportPipelineBuilder
	{
		IReadOnlyList<IArchiveDirectoryImporter> DirectoryImporters { get; }
		IReadOnlyList<IArchiveFileImporter> FileImporters { get; }
		IReadOnlyList<IImportProcessor> ImportProcessors { get; }

		ImportPipeline Build();
		IImportPipelineBuilder UseImporter(IArchiveDirectoryImporter directoryImporter);
		IImportPipelineBuilder UseImporter(IArchiveFileImporter fileImporter);
		IImportPipelineBuilder UseProcessor(IImportProcessor importProcessor);
	}
}
