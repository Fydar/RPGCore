using System.Diagnostics;
using System.IO.Compression;
using System.Threading.Tasks;

namespace RPGCore.Packages.Archives
{
	public class PackedArchive : IArchive
	{
		public ZipArchive ZipArchive { get; }
		public PackedArchiveEntryCollection Files { get; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IReadOnlyArchiveEntryCollection IReadOnlyArchive.Files => Files;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IArchiveEntryCollection IArchive.Files => Files;

		public PackedArchive(ZipArchive zipArchive)
		{
			ZipArchive = zipArchive;
			Files = new PackedArchiveEntryCollection(this);
		}

		public async Task CopyTo(IArchive destination)
		{
			foreach (var file in Files)
			{
				var destFile = destination.Files.GetFile(file.FullName);

				using var openStream = file.OpenRead();
				var dest = file.OpenWrite();
				await openStream.CopyToAsync(dest);
			}
		}
	}
}
