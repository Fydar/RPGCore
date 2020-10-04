using System.Threading.Tasks;

namespace RPGCore.Packages.Archives
{
	public interface IReadOnlyArchiveDirectory : IReadOnlyArchiveEntry
	{
		IReadOnlyArchiveDirectoryCollection Directories { get; }
		IReadOnlyArchiveFileCollection Files { get; }
	}
}
