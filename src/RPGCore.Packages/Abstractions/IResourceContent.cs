using System.IO;

namespace RPGCore.Packages
{
	public interface IResourceContent
	{
		long UncompressedSize { get; }

		Stream OpenRead();
	}
}
