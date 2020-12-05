using RPGCore.FileTree;
using System.Diagnostics;
using System.IO;

namespace RPGCore.Packages
{
	[DebuggerDisplay("{ToString(),nq}")]
	public sealed class PackageResourceContent : IResourceContent
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly IReadOnlyArchiveFile zipArchiveEntry;

		public long UncompressedSize => zipArchiveEntry.UncompressedSize;

		internal PackageResourceContent(IReadOnlyArchiveFile zipArchiveEntry)
		{
			this.zipArchiveEntry = zipArchiveEntry;
		}

		public Stream OpenRead()
		{
			return zipArchiveEntry.OpenRead();
		}

		public override string ToString()
		{
			return new MemorySize(UncompressedSize).ToString();
		}
	}
}
