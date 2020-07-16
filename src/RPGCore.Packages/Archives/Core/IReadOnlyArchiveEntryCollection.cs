using System.Collections.Generic;

namespace RPGCore.Packages.Archives
{
	public interface IReadOnlyArchiveEntryCollection : IEnumerable<IReadOnlyArchiveEntry>
	{
		IReadOnlyArchiveEntry GetFile(string key);
	}
}
