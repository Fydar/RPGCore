using System.Collections.Generic;

namespace RPGCore.Packages.Archives
{
	public interface IReadOnlyArchiveFileCollection : IEnumerable<IReadOnlyArchiveFile>
	{
		IReadOnlyArchiveFile GetFile(string key);
	}
}
