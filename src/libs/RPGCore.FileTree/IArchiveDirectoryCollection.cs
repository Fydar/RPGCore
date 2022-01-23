using System.Collections.Generic;

namespace RPGCore.FileTree;

public interface IArchiveDirectoryCollection : IReadOnlyArchiveDirectoryCollection
{
	new IEnumerable<IArchiveDirectory> All { get; }
	new IArchiveDirectory? GetDirectory(string key);
	IArchiveDirectory GetOrCreateDirectory(string key);
}
