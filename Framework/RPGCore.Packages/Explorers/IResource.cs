using System.IO;
using System.Threading.Tasks;

namespace RPGCore.Packages
{
	public interface IResource
	{
		long CompressedSize { get; }
		string Name { get; }
		long UncompressedSize { get; }

		Stream LoadStream ();
		byte[] LoadData ();
		Task<byte[]> LoadDataAsync ();
	}
}
