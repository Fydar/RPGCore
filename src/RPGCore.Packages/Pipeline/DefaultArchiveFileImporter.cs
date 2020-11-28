using RPGCore.FileTree;
using RPGCore.Packages.Pipeline;
using System.Collections.Generic;

namespace RPGCore.Packages
{
	public sealed class DefaultArchiveFileImporter : IArchiveFileImporter
	{
		public bool CanImport(IArchiveFile archiveFile)
		{
			return true;
		}

		public IEnumerable<ProjectResourceUpdate> ImportFile(ArchiveFileImporterContext context, IArchiveFile archiveFile)
		{
			yield return context.AuthorUpdate(archiveFile.FullName)
				.WithContent(archiveFile);
		}
	}
}
