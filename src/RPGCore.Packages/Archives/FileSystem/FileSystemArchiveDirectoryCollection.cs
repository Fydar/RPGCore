using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace RPGCore.Packages.Archives
{
	public class FileSystemArchiveDirectoryCollection : IArchiveDirectoryCollection
	{
		private readonly FileSystemArchive archive;
		private readonly FileSystemArchiveDirectory owner;

		public FileSystemArchiveDirectoryCollection(FileSystemArchive archive, FileSystemArchiveDirectory owner)
		{
			this.archive = archive;
			this.owner = owner;
		}

		public FileSystemArchiveDirectory GetDirectory(string key)
		{
			return new FileSystemArchiveDirectory(archive, owner,
				new DirectoryInfo(Path.Combine(archive.RootDirectoryInfo.FullName, key)));
		}

		public IEnumerator<FileSystemArchiveDirectory> GetEnumerator()
		{
			foreach (var fileInfo in owner.directoryInfo
				.EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
			{
				yield return new FileSystemArchiveDirectory(archive, owner, fileInfo);
			}
		}

		IEnumerator<IReadOnlyArchiveDirectory> IEnumerable<IReadOnlyArchiveDirectory>.GetEnumerator() => GetEnumerator();
		IEnumerator<IArchiveDirectory> IArchiveDirectoryCollection.GetEnumerator() => GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		IReadOnlyArchiveDirectory IReadOnlyArchiveDirectoryCollection.GetDirectory(string key) => GetDirectory(key);
		IArchiveDirectory IArchiveDirectoryCollection.GetDirectory(string key) => GetDirectory(key);
	}
}
