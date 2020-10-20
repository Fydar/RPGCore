using System.Threading.Tasks;

namespace RPGCore.Packages.Archives
{
	public interface IReadOnlyArchiveEntry
	{
		string Name { get; }
		string FullName { get; }
		IReadOnlyArchive Archive { get; }
		IReadOnlyArchiveDirectory Parent { get; }

		Task CopyInto(IArchiveDirectory destination, string name);
	}
}
