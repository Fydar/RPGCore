using System.IO;
using System.IO.Compression;

namespace RPGCore.Behaviour.Packages
{
	public struct PackageResource
	{
		private readonly PackageExplorer Package;
		private readonly string PackageKey;
		
		public long CompressedSize { get; }
		public long UncompressedSize { get; }

		public string Name
		{
			get
			{
				return PackageKey.Substring(PackageKey.LastIndexOf('/') + 1);
			}
		}

		public PackageResource (PackageExplorer package, ZipArchiveEntry packageEntry)
		{
			Package = package;
			PackageKey = packageEntry.FullName;

			CompressedSize = packageEntry.CompressedLength;
			UncompressedSize = packageEntry.Length;
		}

		public override string ToString ()
		{
			return Name;
		}

		public byte[] LoadData()
		{
			return Package.OpenAsset(PackageKey);
		}
	}
}
