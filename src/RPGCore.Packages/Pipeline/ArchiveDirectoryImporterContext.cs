using RPGCore.Packages.Archives;

namespace RPGCore.Packages
{
	public class ArchiveDirectoryImporterContext
	{
		public ProjectExplorer Explorer { get; internal set; }
		public IArchiveDirectory Source { get; internal set; }
		public string TargetLocation { get; internal set; }
	}
}
