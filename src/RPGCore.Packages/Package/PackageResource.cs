using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace RPGCore.Packages
{
	public sealed class PackageResource : IResource
	{
		private readonly PackageExplorer package;

		public string Name { get; }
		public string FullName { get; }
		public string Extension { get;  }

		public long CompressedSize { get; }
		public long UncompressedSize { get; }

		public PackageResourceTags Tags { get; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		IResourceTags IResource.Tags => Tags;

		internal PackageResource(PackageExplorer package, ZipArchiveEntry packageEntry, IReadOnlyDictionary<string, IReadOnlyList<string>> tagsDocument)
		{
			this.package = package;
			Name = packageEntry.Name;
			FullName = packageEntry.FullName;

			int dotIndex = packageEntry.FullName.LastIndexOf('.');
			Extension = dotIndex != -1
				? packageEntry.FullName.Substring(dotIndex)
				: null;

			CompressedSize = packageEntry.CompressedLength;
			UncompressedSize = packageEntry.Length;

			Tags = new PackageResourceTags(tagsDocument, this);
		}

		public Stream LoadStream()
		{
			return package.LoadStream(FullName);
		}

		public byte[] LoadData()
		{
			return package.OpenAsset(FullName);
		}

		public Task<byte[]> LoadDataAsync()
		{
			var pkg = package;
			string pkgKey = FullName;
			return Task.Run(() => pkg.OpenAsset(pkgKey));
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
