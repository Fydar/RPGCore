using RPGCore.Packages.Archives;
using RPGCore.Packages.Pipeline;

namespace RPGCore.Packages
{
	public class ArchiveFileImporterContext
	{
		public ProjectExplorer Explorer { get; internal set; }
		public IArchiveFile Source { get; internal set; }

		public ProjectResourceUpdate AuthorUpdate(string name)
		{
			return new ProjectResourceUpdate(Explorer, name);
		}
	}
}
