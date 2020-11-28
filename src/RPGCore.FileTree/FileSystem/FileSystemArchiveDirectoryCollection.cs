using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace RPGCore.FileTree.FileSystem
{
	public class FileSystemArchiveDirectoryCollection : IArchiveDirectoryCollection
	{
		private readonly FileSystemArchive archive;
		private readonly FileSystemArchiveDirectory owner;
		internal readonly List<FileSystemArchiveDirectory> internalList;

		public FileSystemArchiveDirectoryCollection(FileSystemArchive archive, FileSystemArchiveDirectory owner)
		{
			this.archive = archive;
			this.owner = owner;

			internalList = new List<FileSystemArchiveDirectory>();
			foreach (var fileInfo in owner.directoryInfo
				.EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
			{
				internalList.Add(new FileSystemArchiveDirectory(archive, owner, fileInfo));
			}
		}

		public FileSystemArchiveDirectory GetDirectory(string key)
		{
			foreach (var directory in internalList)
			{
				if (directory.Name == key)
				{
					return directory;
				}
			}
			return null;
		}

		public IEnumerator<FileSystemArchiveDirectory> GetEnumerator()
		{
			return internalList.GetEnumerator();
		}

		IEnumerator<IReadOnlyArchiveDirectory> IEnumerable<IReadOnlyArchiveDirectory>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator<IArchiveDirectory> IArchiveDirectoryCollection.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		IReadOnlyArchiveDirectory IReadOnlyArchiveDirectoryCollection.GetDirectory(string key)
		{
			return GetDirectory(key);
		}

		IArchiveDirectory IArchiveDirectoryCollection.GetDirectory(string key)
		{
			return GetDirectory(key);
		}
	}
}
