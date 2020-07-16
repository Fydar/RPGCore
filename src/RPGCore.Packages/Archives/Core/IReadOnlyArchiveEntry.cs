using System.IO;

namespace RPGCore.Packages.Archives
{
	public interface IReadOnlyArchiveEntry
	{
		string Name { get; }
		string FullName { get; }
		string Extension { get; }
		long CompressedSize { get; }
		long UncompressedSize { get; }

		Stream OpenRead();
	}
}
