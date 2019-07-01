using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace RPGCore.Packages
{
	public class PackageResource : IResource
	{
		private readonly PackageExplorer Package;

		public string Name { get; }
		public string FullName { get; }

		public long CompressedSize { get; }
		public long UncompressedSize { get; }

		public PackageResource (PackageExplorer package, ZipArchiveEntry packageEntry)
		{
			Package = package;
			Name = packageEntry.Name;
			FullName = packageEntry.FullName;

			CompressedSize = packageEntry.CompressedLength;
			UncompressedSize = packageEntry.Length;
		}

		public Stream LoadStream ()
		{
			return Package.LoadStream (FullName);
		}

		public byte[] LoadData ()
		{
			return Package.OpenAsset (FullName);
		}

		public Task<byte[]> LoadDataAsync ()
		{
			var pkg = Package;
			string pkgKey = FullName;
			return Task.Run (() => pkg.OpenAsset (pkgKey));
		}

		public override string ToString ()
		{
			return Name;
		}
	}
}
