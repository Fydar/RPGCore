using System.Collections.Generic;

namespace RPGCore.FileTree
{
	public interface IArchiveFileCollection : IReadOnlyArchiveFileCollection
	{
		new IArchiveFile GetFile(string key);
		new IEnumerator<IArchiveFile> GetEnumerator();
	}
}
