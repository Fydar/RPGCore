using System.Collections.Generic;

namespace RPGCore.Packages.Archives
{
	public interface IArchiveFileCollection : IReadOnlyArchiveFileCollection
	{
		new IArchiveFile GetFile(string key);
		new IEnumerator<IArchiveFile> GetEnumerator();
	}
}
