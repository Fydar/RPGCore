using System.Diagnostics;
using System.IO;
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
			FullName = fileInfo.FullName
				.Substring(archive.RootDirectoryInfo.FullName.Length + 1)
				.Replace('\\', '/');
			Extension = fileInfo.Extension;
		}

		Task IArchiveEntry.MoveInto(IArchiveDirectory destination, string name)
		{
			throw new System.NotImplementedException();
		}

		Task IReadOnlyArchiveEntry.CopyInto(IArchiveDirectory destination, string name)
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

		public Task CopyInto(IArchiveDirectory destination, string name)
		{
			return null;
		}
	}
}
