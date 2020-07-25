using RPGCore.Packages.Archives;

namespace RPGCore.Packages
{
	public abstract class ImportFilter
	{
		public abstract bool AllowFile(IArchiveEntry archiveEntry);
	}
}
