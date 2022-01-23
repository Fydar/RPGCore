using System.Threading.Tasks;

namespace RPGCore.FileTree;

public interface IArchiveEntry : IReadOnlyArchiveEntry
{
	new IArchive Archive { get; }
	new IArchiveDirectory? Parent { get; }

	Task MoveInto(IArchiveDirectory destination, string name);
}
