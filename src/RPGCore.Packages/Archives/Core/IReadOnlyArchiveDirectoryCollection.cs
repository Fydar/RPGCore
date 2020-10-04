using System.Collections.Generic;

namespace RPGCore.Packages.Archives
{
	public interface IReadOnlyArchiveDirectoryCollection : IEnumerable<IReadOnlyArchiveDirectory>
	{
		IReadOnlyArchiveDirectory GetDirectory(string key);
	}
}
