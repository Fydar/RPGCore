using System.Threading.Tasks;

namespace RPGCore.Packages.Archives
{
	public interface IArchiveEntry : IReadOnlyArchiveEntry
	{
		new IArchive Archive { get; }
		new IArchiveDirectory Parent { get; }

		Task MoveInto(IArchiveDirectory destination, string name);
	}
}
