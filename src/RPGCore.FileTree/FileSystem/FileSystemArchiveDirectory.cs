using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace RPGCore.FileTree.FileSystem
{
	public class FileSystemArchiveDirectory : IArchiveDirectory
	{
		internal readonly DirectoryInfo directoryInfo;

		public string Name { get; internal set; }
		public string FullName { get; internal set; }
		public FileSystemArchive Archive { get; }
		public FileSystemArchiveDirectory Parent { get; internal set; }
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
			Archive = archive;
			Parent = parent;
			this.directoryInfo = directoryInfo;

			Directories = new FileSystemArchiveDirectoryCollection(archive, this);
			Files = new FileSystemArchiveFileCollection(archive, this);

			if (parent != null)
			{
				Name = directoryInfo.Name;
				FullName = MakeFullName(parent, directoryInfo.Name);
			}
		}

		public Task CopyIntoAsync(IArchiveDirectory destination, string name)
		{
			static void CopyIntoRecursive(FileSystemArchiveDirectory from, IArchiveDirectory to, string rename)
			{
				foreach (var fromFile in from.Files)
				{
					var toFile = to.Files.GetFile(fromFile.Name);

					if (toFile is FileSystemArchiveFile toFileSystemFile)
					{
						fromFile.FileInfo.CopyTo(toFileSystemFile.FileInfo.FullName, true);
					}
					else
					{
						using var readStream = fromFile.OpenRead();
						using var writeStream = toFile.OpenWrite();

						readStream.CopyTo(writeStream);
					}
				}
				foreach (var childFromDirectory in from.Directories)
				{
					var childToDirectory = to.Directories.GetDirectory(childFromDirectory.Name);

					CopyIntoRecursive(childFromDirectory, childToDirectory, childFromDirectory.Name);
				}
			}

			CopyIntoRecursive(this, destination, name);

			return Task.CompletedTask;
		}

		public Task MoveInto(IArchiveDirectory destination, string name)
		{
			throw new System.NotImplementedException();
		}

		private static string MakeFullName(IArchiveDirectory parent, string key)
		{
			if (parent == null || string.IsNullOrEmpty(parent.FullName))
			{
				return key;
			}
			else
			{
				return $"{parent.FullName}/{key}";
			}
		}

		public override string ToString()
		{
			return FullName;
		}
	}
}
