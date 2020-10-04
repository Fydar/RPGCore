using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace RPGCore.Packages.Archives
{
	public class FileSystemArchiveFileCollection : IArchiveFileCollection
	{
		private readonly FileSystemArchive archive;
		private readonly FileSystemArchiveDirectory owner;

		public FileSystemArchiveFileCollection(FileSystemArchive archive, FileSystemArchiveDirectory owner)
		{
			this.archive = archive;
			this.owner = owner;
		}

		public FileSystemArchiveFile GetFile(string key)
		{
			return new FileSystemArchiveFile(archive, owner,
				new FileInfo(Path.Combine(archive.RootDirectoryInfo.FullName, key)));
		}

		public IEnumerator<FileSystemArchiveFile> GetEnumerator()
		{
			foreach (var fileInfo in owner.directoryInfo
				.EnumerateFiles("*", SearchOption.TopDirectoryOnly))
			{
				yield return new FileSystemArchiveFile(archive, owner, fileInfo);
			}
		}

		IEnumerator<IReadOnlyArchiveFile> IEnumerable<IReadOnlyArchiveFile>.GetEnumerator() => GetEnumerator();
		IEnumerator<IArchiveFile> IArchiveFileCollection.GetEnumerator() => GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		IReadOnlyArchiveFile IReadOnlyArchiveFileCollection.GetFile(string key) => GetFile(key);
		IArchiveFile IArchiveFileCollection.GetFile(string key) => GetFile(key);
	}
}
