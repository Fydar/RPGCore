using RPGCore.FileTree;
using RPGCore.Packages.Pipeline;
using System.Collections.Generic;

namespace RPGCore.Packages
{
	public interface IArchiveDirectoryImporter
	{
		bool CanImport(IArchiveDirectory archiveDirectory);

		IEnumerable<ProjectResourceUpdate> ImportDirectory(ArchiveDirectoryImporterContext context, IArchiveDirectory archiveDirectory);
	}
}
