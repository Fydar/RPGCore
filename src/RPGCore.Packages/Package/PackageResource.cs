using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace RPGCore.Packages
{
	public sealed class PackageResource : IResource
	{
		private readonly PackageExplorer Package;

		public string Name { get; }
		public string FullName { get; }

		public long CompressedSize { get; }
		public long UncompressedSize { get; }

		public PackageResourceTags Tags { get; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		IResourceTags IResource.Tags => Tags;

		public PackageResource(PackageExplorer package, ZipArchiveEntry packageEntry, IReadOnlyDictionary<string, IReadOnlyList<string>> tagsDocument)
		{
			Package = package;
			Name = packageEntry.Name;
			FullName = packageEntry.FullName;

			CompressedSize = packageEntry.CompressedLength;
			UncompressedSize = packageEntry.Length;

			Tags = new PackageResourceTags(tagsDocument, this);
		}

		public Stream LoadStream()
		{
			return Package.LoadStream(FullName);
		}

		public byte[] LoadData()
		{
			return Package.OpenAsset(FullName);
		}

		public Task<byte[]> LoadDataAsync()
		{
			var pkg = Package;
			string pkgKey = FullName;
			return Task.Run(() => pkg.OpenAsset(pkgKey));
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
