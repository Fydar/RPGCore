using System.Collections.Generic;

namespace RPGCore.FileTree;

public interface IReadOnlyArchiveFileCollection : IEnumerable<IReadOnlyArchiveFile>
{
	IReadOnlyArchiveFile GetFile(string key);
}
