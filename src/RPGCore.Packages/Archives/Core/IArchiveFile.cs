using System.IO;
using System.Threading.Tasks;

namespace RPGCore.Packages.Archives
{
	public interface IArchiveFile : IReadOnlyArchiveFile, IArchiveEntry
	{
		Stream OpenWrite();
		Task DeleteAsync();
	}
}
