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

		public PackageExplorer Explorer { get; }
		public PackageDirectory Directory { get; internal set; }
		public PackageResourceDependencies Dependencies { get; }
		public PackageResourceTags Tags { get; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IExplorer IResource.Explorer => Explorer;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IDirectory IResource.Directory => Directory;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IResourceDependencies IResource.Dependencies => Dependencies;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IResourceTags IResource.Tags => Tags;

		internal PackageResource(PackageExplorer packageExplorer, ZipArchiveEntry contentEntry, PackageResourceMetadataModel metadataModel)
		{
			Explorer = packageExplorer;
			Dependencies = new PackageResourceDependencies(packageExplorer, metadataModel);
			Tags = new PackageResourceTags();

			Name = contentEntry.Name;

			string withoutData = contentEntry.FullName.Substring(5);
			FullName = withoutData;

			int dotIndex = contentEntry.FullName.LastIndexOf('.');
			Extension = dotIndex != -1
				? contentEntry.FullName.Substring(dotIndex)
				: null;

			CompressedSize = contentEntry.CompressedLength;
			UncompressedSize = contentEntry.Length;
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
