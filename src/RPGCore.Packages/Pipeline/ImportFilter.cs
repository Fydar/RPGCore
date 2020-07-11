using System.IO;

namespace RPGCore.Packages
{
	public abstract class ImportFilter
	{
		public abstract bool AllowFile(FileInfo file);
	}
}
