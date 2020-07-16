using System.IO;
using System.Threading.Tasks;

namespace RPGCore.Packages.Archives
{
	public interface IArchiveEntry : IReadOnlyArchiveEntry
	{
		Stream OpenWrite();

		Task DeleteAsync();
		Task RenameAsync(string destination);
		Task<IArchiveEntry> DuplicateAsync(string destination);
		Task UpdateAsync(Stream content);
	}
}
