using RPGCore.Packages.Archives;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace RPGCore.Packages
{
	[DebuggerDisplay("{ToString(),nq}")]
	public sealed class PackageResourceContent : IResourceContent
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly PackageExplorer packageExplorer;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly string contentKey;

		public long CompressedSize { get; }
		public long UncompressedSize { get; }

		internal PackageResourceContent(PackageExplorer packageExplorer, string contentKey, IReadOnlyArchiveEntry zipArchiveEntry)
		{
			this.packageExplorer = packageExplorer;
			this.contentKey = contentKey;

			CompressedSize = zipArchiveEntry.CompressedSize;
			UncompressedSize = zipArchiveEntry.UncompressedSize;
		}

		public Stream LoadStream()
		{
			return packageExplorer.LoadStream(contentKey);
		}

		public byte[] LoadData()
		{
			return packageExplorer.OpenAsset(contentKey);
		}

		public Task<byte[]> LoadDataAsync()
		{
			var pkg = packageExplorer;
			string pkgKey = contentKey;
			return Task.Run(() => pkg.OpenAsset(pkgKey));
		}

		public override string ToString()
		{
			return new MemorySize(UncompressedSize).ToString();
		}
	}
}
