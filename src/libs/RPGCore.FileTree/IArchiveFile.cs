using System.IO;
using System.Threading.Tasks;

namespace RPGCore.FileTree;

public interface IArchiveFile : IReadOnlyArchiveFile, IArchiveEntry
{
	Stream OpenWrite();
	Task DeleteAsync();
}
