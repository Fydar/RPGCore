using System.IO;
using System.Threading.Tasks;

namespace RPGCore.Packages.Archives
{
	public class FileSystemArchiveEntry : IArchiveEntry
	{
		public FileSystemArchive Archive { get; }
		public FileInfo FileInfo { get; }

		public string Name { get; }
		public string FullName { get; }
		public string Extension { get; }

		public long CompressedSize => FileInfo.Length;
		public long UncompressedSize => FileInfo.Length;

		public FileSystemArchiveEntry(FileSystemArchive archive, FileInfo fileInfo)
		{
			Archive = archive;
			FileInfo = fileInfo;

			Name = fileInfo.Name;
			FullName = fileInfo.FullName.Replace(archive.DirectoryInfo.FullName + "\\", "")
				.Replace('\\', '/');
			Extension = fileInfo.Extension;
		}

		public Task DeleteAsync()
		{
			return Task.Run(() => FileInfo.Delete());
		}

		public Task RenameAsync(string destination)
		{
			return Task.Run(() => FileInfo.MoveTo(destination));
		}

		public Task<FileSystemArchiveEntry> DuplicateAsync(string destination)
		{
			return Task.Run(() =>
			{
				var destinationFile = new FileInfo(destination);
				FileInfo.CopyTo(destination);
				return new FileSystemArchiveEntry(Archive, destinationFile);
			});
		}

		async Task<IArchiveEntry> IArchiveEntry.DuplicateAsync(string destination)
		{
			var result = await DuplicateAsync(destination);
			return result;
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
	}
}
