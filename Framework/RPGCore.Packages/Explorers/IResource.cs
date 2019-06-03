using System.IO;
using System.Threading.Tasks;

namespace RPGCore.Packages
{
	public interface IResource
	{
		long CompressedSize { get; }
		string Name { get; }
		long UncompressedSize { get; }

		PackageStream LoadStream ();
		byte[] LoadData ();
		Task<byte[]> LoadDataAsync ();
	}
}
