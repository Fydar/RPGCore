using RPGCore.Packages.Archives;
using System.IO;

namespace RPGCore.Packages
{
	public abstract class ImportFilter
	{
		public abstract bool AllowFile(IArchiveEntry archiveEntry);
	}
}
