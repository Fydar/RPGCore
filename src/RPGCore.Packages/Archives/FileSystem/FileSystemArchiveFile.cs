using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace RPGCore.Packages.Archives
{
	public class FileSystemArchiveFile : IArchiveFile
	{
		public FileSystemArchive Archive { get; }
		public FileInfo FileInfo { get; }

		public string Name { get; }
		public string FullName { get; }
		public string Extension { get; }

		public long CompressedSize => FileInfo.Length;
		public long UncompressedSize => FileInfo.Length;

		public FileSystemArchiveDirectory Parent { get; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IReadOnlyArchive IReadOnlyArchiveEntry.Archive => Archive;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IReadOnlyArchiveDirectory IReadOnlyArchiveEntry.Parent => Parent;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IArchive IArchiveEntry.Archive => Archive;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IArchiveDirectory IArchiveEntry.Parent => Parent;

		public FileSystemArchiveFile(FileSystemArchive archive, FileSystemArchiveDirectory parent, FileInfo fileInfo)
		{
			Archive = archive;
			Parent = parent;
			FileInfo = fileInfo;

			Name = fileInfo.Name;
			FullName = MakeFullName(parent, fileInfo.Name);
			Extension = fileInfo.Extension;
		}

		Task IArchiveEntry.MoveInto(IArchiveDirectory destination, string name)
		{
			throw new System.NotImplementedException();
		}

		public Task DeleteAsync()
		{
			return Task.Run(() => FileInfo.Delete());
		}

		public Task RenameAsync(string destination)
		{
			return Task.Run(() => FileInfo.MoveTo(destination));
		}

		public Stream OpenRead()
		{
			return FileInfo.OpenRead();
		}

		public override string ToString()
		{
			return FullName;
		}

		public Stream OpenWrite()
		{
			FileInfo.Directory.Create();
			return FileInfo.Open(FileMode.Create, FileAccess.Write);
		}

		public async Task CopyInto(IArchiveDirectory destination, string name)
		{
			var toFile = destination.Files.GetFile(Name);

			if (toFile is FileSystemArchiveFile toFileSystemFile)
			{
				FileInfo.CopyTo(toFileSystemFile.FileInfo.FullName, true);
			}
			if (toFile is PackedArchiveFile toParckedFile)
			{
				toParckedFile.Archive.ZipArchive.CreateEntryFromFile(FileInfo.FullName, toFile.FullName);
			}
			else
			{
				using var readStream = OpenRead();
				using var writeStream = toFile.OpenWrite();

				readStream.CopyTo(writeStream);
			}
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
	}
}
