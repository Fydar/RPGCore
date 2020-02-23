using System.IO;
using System.Threading.Tasks;

namespace RPGCore.Packages
{
	public interface IResource
	{
		string Name { get; }
		string FullName { get; }
		long CompressedSize { get; }
		long UncompressedSize { get; }
		IResourceTags Tags { get; }

		Stream LoadStream();

		byte[] LoadData();

		Task<byte[]> LoadDataAsync();
	}
}
