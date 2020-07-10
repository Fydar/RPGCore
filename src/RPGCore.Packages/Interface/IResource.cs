using System.IO;
using System.Threading.Tasks;

namespace RPGCore.Packages
{
	public interface IResource
	{
		string Name { get; }
		string FullName { get; }
		string Extension { get; }
		long UncompressedSize { get; }
		IResourceTags Tags { get; }
		IDirectory Directory { get; }
		IResourceDependencies Dependencies { get; }
		IExplorer Explorer { get; }

		Stream LoadStream();

		byte[] LoadData();

		Task<byte[]> LoadDataAsync();
	}
}
