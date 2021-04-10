using RPGCore.FileTree;

namespace RPGCore.Projects
{
	public class ArchiveDirectoryImporterContext
	{
		public ProjectExplorer Explorer { get; internal set; }
		public IArchiveDirectory Source { get; internal set; }
		public string TargetLocation { get; internal set; }
	}
}
