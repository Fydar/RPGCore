using System.Collections.Generic;

namespace RPGCore.Packages.Archives
{
	public interface IArchiveDirectoryCollection : IReadOnlyArchiveDirectoryCollection
	{
		new IArchiveDirectory GetDirectory(string key);
		new IEnumerator<IArchiveDirectory> GetEnumerator();
	}
}
