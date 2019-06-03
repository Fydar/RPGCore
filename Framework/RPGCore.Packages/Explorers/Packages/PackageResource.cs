using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace RPGCore.Packages
{
	public class PackageResource : IResource
	{
		private readonly PackageExplorer Package;
		private readonly string PackageKey;

		public long CompressedSize { get; }
		public long UncompressedSize { get; }

		public string Name { get; }

		public PackageResource (PackageExplorer package, ZipArchiveEntry packageEntry)
		{
			Package = package;
			PackageKey = packageEntry.FullName;

			CompressedSize = packageEntry.CompressedLength;
			UncompressedSize = packageEntry.Length;
			
			Name = packageEntry.Name;
		}

		public PackageStream LoadStream()
		{
			return Package.LoadStream(PackageKey);
		}

		public byte[] LoadData ()
		{
			return Package.OpenAsset (PackageKey);
		}

		public Task<byte[]> LoadDataAsync ()
		{
			var pkg = Package;
			string pkgKey = PackageKey;
			return Task.Run (() => pkg.OpenAsset (pkgKey));
		}

		public override string ToString ()
		{
			return Name;
		}
	}
}
