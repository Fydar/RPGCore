using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace RPGCore.Packages.Archives
{
	public class FileSystemArchiveEntryCollection : IArchiveEntryCollection
	{
		private readonly FileSystemArchive archive;

		public FileSystemArchiveEntryCollection(FileSystemArchive archive)
		{
			this.archive = archive;
		}

		public FileSystemArchiveEntry GetFile(string key)
		{
			return new FileSystemArchiveEntry(archive,
				new FileInfo(Path.Combine(archive.DirectoryInfo.FullName, key)));
		}

		public IEnumerator<FileSystemArchiveEntry> GetEnumerator()
		{
			foreach (var fileInfo in archive.DirectoryInfo
				.EnumerateFiles("*", SearchOption.AllDirectories))
			{
				yield return new FileSystemArchiveEntry(archive, fileInfo);
			}
		}

		IEnumerator<IReadOnlyArchiveEntry> IEnumerable<IReadOnlyArchiveEntry>.GetEnumerator() => GetEnumerator();
		IEnumerator<IArchiveEntry> IEnumerable<IArchiveEntry>.GetEnumerator() => GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		IReadOnlyArchiveEntry IReadOnlyArchiveEntryCollection.GetFile(string key) => GetFile(key);
		IArchiveEntry IArchiveEntryCollection.GetFile(string key) => GetFile(key);
	}
}
