using RPGCore.FileTree;
using RPGCore.FileTree;
using RPGCore.Packages.Pipeline;
using System.Collections.Generic;
using System.Linq;

namespace RPGCore.Packages.Extensions.MetaFiles
{
	internal class JsonMetaFileSuffixImporter : IArchiveFileImporter
	{
		private readonly string suffix;

		public JsonMetaFileSuffixImporter(string suffix)
		{
			this.suffix = suffix;
		}

		public bool CanImport(IArchiveFile archiveFile)
		{
			return archiveFile.Extension == suffix;
		}

		public IEnumerable<ProjectResourceUpdate> ImportFile(ArchiveFileImporterContext context, IArchiveFile archiveFile)
		{
			return Enumerable.Empty<ProjectResourceUpdate>();
		}
	}
}
