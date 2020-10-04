using System.Diagnostics;
using System.IO;

namespace RPGCore.Packages.Archives
{
	public class FileSystemArchive : IArchive
	{
		public DirectoryInfo RootDirectoryInfo { get; }
		public FileSystemArchiveDirectory RootDirectory { get; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IArchiveDirectory IArchive.RootDirectory => RootDirectory;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IReadOnlyArchiveDirectory IReadOnlyArchive.RootDirectory => RootDirectory;

		public int MaximumWriteThreads => 6;

		public FileSystemArchive(DirectoryInfo rootDirectory)
		{
			RootDirectoryInfo = rootDirectory;
			RootDirectory = new FileSystemArchiveDirectory(this, null, rootDirectory);
		}
	}
}
