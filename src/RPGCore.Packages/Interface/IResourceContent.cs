using System.IO;
using System.Threading.Tasks;

namespace RPGCore.Packages
{
	public interface IResourceContent
	{
		long UncompressedSize { get; }

		Stream LoadStream();

		byte[] LoadData();

		Task<byte[]> LoadDataAsync();
	}
}
