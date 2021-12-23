using System.Threading.Tasks;

namespace RPGCore.FileTree
{
	public interface IReadOnlyArchiveEntry
	{
		string Name { get; }
		string FullName { get; }
		IReadOnlyArchive Archive { get; }
		IReadOnlyArchiveDirectory? Parent { get; }

		Task CopyIntoAsync(IArchiveDirectory destination, string name);
	}
}
