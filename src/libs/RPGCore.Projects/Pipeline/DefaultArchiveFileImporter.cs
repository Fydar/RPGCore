using RPGCore.FileTree;
using RPGCore.Projects.Pipeline;
using System.Collections.Generic;

namespace RPGCore.Projects
{
	public sealed class DefaultArchiveFileImporter : IArchiveFileImporter
	{
		/// <inheritdoc/>
		public bool CanImport(IArchiveFile archiveFile)
		{
			return true;
		}

		/// <inheritdoc/>
		public IEnumerable<ProjectResourceUpdate> ImportFile(ArchiveFileImporterContext context, IArchiveFile archiveFile)
		{
			yield return context.AuthorUpdate(archiveFile.FullName)
				.WithContent(archiveFile);
		}
	}
}
