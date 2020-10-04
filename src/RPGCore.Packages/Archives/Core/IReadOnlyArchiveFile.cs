using System.IO;

namespace RPGCore.Packages.Archives
{
	public interface IReadOnlyArchiveFile : IReadOnlyArchiveEntry
	{
		string Extension { get; }
		long UncompressedSize { get; }

		Stream OpenRead();
	}
}
