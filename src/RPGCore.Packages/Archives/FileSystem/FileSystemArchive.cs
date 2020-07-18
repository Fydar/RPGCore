using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace RPGCore.Packages.Archives
{
	public class FileSystemArchive : IArchive
	{
		public DirectoryInfo DirectoryInfo { get; }
		public FileSystemArchiveEntryCollection Files { get; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IArchiveEntryCollection IArchive.Files => Files;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IReadOnlyArchiveEntryCollection IReadOnlyArchive.Files => Files;

		public FileSystemArchive(DirectoryInfo directoryInfo)
		{
			DirectoryInfo = directoryInfo;
			Files = new FileSystemArchiveEntryCollection(this);
		}

		public async Task CopyTo(IArchive destination)
		{
			foreach (var file in Files)
			{
				var destFile = destination.Files.GetFile(file.FullName);

				using var openStream = file.OpenRead();
				var dest = file.OpenWrite();
				await  openStream.CopyToAsync(dest);
			}
		}
	}
}
