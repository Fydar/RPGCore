using RPGCore.FileTree;
using System.Diagnostics;
using System.IO;

namespace RPGCore.Packages
{
	/// <summary>
	/// Represents the content of a resource in a package.
	/// </summary>
	[DebuggerDisplay("{ToString(),nq}")]
	public sealed class PackageResourceContent : IResourceContent
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly IReadOnlyArchiveFile zipArchiveEntry;

		/// <inheritdoc/>
		public long UncompressedSize => zipArchiveEntry.UncompressedSize;

		internal PackageResourceContent(IReadOnlyArchiveFile zipArchiveEntry)
		{
			this.zipArchiveEntry = zipArchiveEntry;
		}

		/// <inheritdoc/>
		public Stream OpenRead()
		{
			return zipArchiveEntry.OpenRead();
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return new MemorySize(UncompressedSize).ToString();
		}
	}
}
