using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace RPGCore.Packages.Archives
{
	public class FileSystemArchiveDirectory : IArchiveDirectory
	{
		internal readonly DirectoryInfo directoryInfo;

		public string Name => directoryInfo.Name;
		public string FullName => "";
		public string ArchiveKey => "";
		public FileSystemArchive Archive { get; }
		public FileSystemArchiveDirectory Parent { get; }
		public FileSystemArchiveDirectoryCollection Directories { get; }
		public FileSystemArchiveFileCollection Files { get; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IArchiveDirectoryCollection IArchiveDirectory.Directories => Directories;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IArchiveFileCollection IArchiveDirectory.Files => Files;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IArchiveDirectory IArchiveEntry.Parent => Parent;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IArchive IArchiveEntry.Archive => Archive;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IReadOnlyArchiveDirectoryCollection IReadOnlyArchiveDirectory.Directories => Directories;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IReadOnlyArchiveFileCollection IReadOnlyArchiveDirectory.Files => Files;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IReadOnlyArchiveDirectory IReadOnlyArchiveEntry.Parent => Parent;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IReadOnlyArchive IReadOnlyArchiveEntry.Archive => Archive;

		internal FileSystemArchiveDirectory(FileSystemArchive archive, FileSystemArchiveDirectory parent, DirectoryInfo directoryInfo)
		{
			this.Archive = archive;
			Parent = parent;
			this.directoryInfo = directoryInfo;

			Directories = new FileSystemArchiveDirectoryCollection(archive, this);
			Files = new FileSystemArchiveFileCollection(archive, this);
		}

		public Task CopyInto(IArchiveDirectory destination, string name)
		{
			static void CopyIntoRecursive(FileSystemArchiveDirectory from, IArchiveDirectory to, string rename)
			{
				foreach (var file in from.Files)
				{
					var destFile = to.Files.GetFile(file.Name);

					if (destFile is FileSystemArchiveFile fileSystemFile)
					{
						file.FileInfo.CopyTo(fileSystemFile.FileInfo.FullName, true);
					}
					else
					{
						using var openStream = file.OpenRead();
						var dest = file.OpenWrite();
						openStream.CopyTo(dest);
					}
				}
				foreach (var directory in from.Directories)
				{
					var destDirectory = to.Directories.GetDirectory(directory.Name);

					CopyIntoRecursive(directory, destDirectory, directory.Name);
				}
			}

			return Task.CompletedTask;
		}

		public Task MoveInto(IArchiveDirectory destination, string name)
		{
			throw new System.NotImplementedException();
		}
	}
}
