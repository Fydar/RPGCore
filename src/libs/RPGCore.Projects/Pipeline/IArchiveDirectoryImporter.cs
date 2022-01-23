using RPGCore.FileTree;
using RPGCore.Projects.Pipeline;
using System.Collections.Generic;

namespace RPGCore.Projects;

public interface IArchiveDirectoryImporter
{
	bool CanImport(IArchiveDirectory archiveDirectory);

	IEnumerable<ProjectResourceUpdate> ImportDirectory(ArchiveDirectoryImporterContext context, IArchiveDirectory archiveDirectory);
}
