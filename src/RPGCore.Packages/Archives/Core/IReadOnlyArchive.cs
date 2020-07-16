using System.Threading.Tasks;

namespace RPGCore.Packages.Archives
{
	public interface IReadOnlyArchive
	{
		IReadOnlyArchiveEntryCollection Files { get; }

		Task CopyTo(IArchive destination);
	}
}
