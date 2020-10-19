using RPGCore.Packages.Archives;
using RPGCore.Packages.Pipeline;
using System.Collections.Generic;

namespace RPGCore.Packages
{
	public interface IArchiveFileImporter
	{
		bool CanImport(IArchiveFile archiveFile);

		IEnumerable<ProjectResourceUpdate> ImportFile(ArchiveFileImporterContext context, IArchiveFile archiveFile);
	}
}
