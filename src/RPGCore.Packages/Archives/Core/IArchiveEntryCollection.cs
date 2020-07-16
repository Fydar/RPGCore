using System.Collections.Generic;

namespace RPGCore.Packages.Archives
{
	public interface IArchiveEntryCollection : IReadOnlyArchiveEntryCollection, IEnumerable<IArchiveEntry>
	{
		new IArchiveEntry GetFile(string key);
	}
}
