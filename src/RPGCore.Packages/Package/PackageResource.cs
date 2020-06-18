using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace RPGCore.Packages
{
	public sealed class PackageResource : IResource
	{
		public string Name { get; }
		public string FullName { get; }
		public string Extension { get; }

		public long CompressedSize { get; }
		public long UncompressedSize { get; }

		public PackageResourceTags Tags { get; }
		public PackageExplorer Explorer { get; }
		public IDirectory Directory { get; internal set; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IResourceTags IResource.Tags => Tags;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IExplorer IResource.Explorer => Explorer;

		internal PackageResource(PackageExplorer explorer, ZipArchiveEntry packageEntry)
		{
			Tags = new PackageResourceTags();
			Explorer = explorer;
			Name = packageEntry.Name;

			string withoutData = packageEntry.FullName.Substring(5);
			FullName = withoutData;

			int dotIndex = packageEntry.FullName.LastIndexOf('.');
			Extension = dotIndex != -1
				? packageEntry.FullName.Substring(dotIndex)
				: null;

			CompressedSize = packageEntry.CompressedLength;
			UncompressedSize = packageEntry.Length;
		}

		public Stream LoadStream()
		{
			return Explorer.LoadStream(FullName);
		}

		public byte[] LoadData()
		{
			return Explorer.OpenAsset(FullName);
		}

		public Task<byte[]> LoadDataAsync()
		{
			var pkg = Explorer;
			string pkgKey = FullName;
			return Task.Run(() => pkg.OpenAsset(pkgKey));
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
