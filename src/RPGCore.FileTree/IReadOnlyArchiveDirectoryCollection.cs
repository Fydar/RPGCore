using System.Collections.Generic;

namespace RPGCore.FileTree
{
	public interface IReadOnlyArchiveDirectoryCollection : IEnumerable<IReadOnlyArchiveDirectory>
	{
		IReadOnlyArchiveDirectory GetDirectory(string key);
	}
}
