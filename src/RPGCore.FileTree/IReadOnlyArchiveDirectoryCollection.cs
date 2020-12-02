using System.Collections.Generic;

namespace RPGCore.FileTree
{
	public interface IReadOnlyArchiveDirectoryCollection
	{
		IEnumerable<IReadOnlyArchiveDirectory> All { get; }

		IReadOnlyArchiveDirectory GetDirectory(string key);
	}
}
