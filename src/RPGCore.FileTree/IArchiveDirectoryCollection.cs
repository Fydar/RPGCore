using System.Collections.Generic;

namespace RPGCore.FileTree
{
	public interface IArchiveDirectoryCollection : IReadOnlyArchiveDirectoryCollection
	{
		new IArchiveDirectory GetDirectory(string key);
		new IEnumerator<IArchiveDirectory> GetEnumerator();
	}
}
