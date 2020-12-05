using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace RPGCore.FileTree.FileSystem
{
	public class FileSystemArchiveFileCollection : IArchiveFileCollection
	{
		private readonly FileSystemArchive archive;
		private readonly FileSystemArchiveDirectory owner;
		internal readonly List<FileSystemArchiveFile> internalList;

		public FileSystemArchiveFileCollection(FileSystemArchive archive, FileSystemArchiveDirectory owner)
		{
			this.archive = archive;
			this.owner = owner;

			internalList = new List<FileSystemArchiveFile>();
			foreach (var fileInfo in owner.directoryInfo
				.EnumerateFiles("*", SearchOption.TopDirectoryOnly))
			{
				internalList.Add(new FileSystemArchiveFile(archive, owner, fileInfo));
			}
		}

		public FileSystemArchiveFile GetFile(string key)
		{
			for (int i = 0; i < internalList.Count; i++)
			{
				var file = internalList[i];
				if (file.Name == key)
				{
					return file;
				}
			}
			return null;
		}

		public FileSystemArchiveFile GetOrCreateFile(string key)
		{
			var file = GetFile(key);

			if (file == null)
			{
				var info = new FileInfo(Path.Combine(owner.directoryInfo.FullName, key));
				// info.Create();
				file = new FileSystemArchiveFile(archive, owner, info);
				internalList.Add(file);
			}

			return file;
		}

		public IEnumerator<FileSystemArchiveFile> GetEnumerator()
		{
			return internalList.GetEnumerator();
		}

		IEnumerator<IReadOnlyArchiveFile> IEnumerable<IReadOnlyArchiveFile>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator<IArchiveFile> IArchiveFileCollection.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		IReadOnlyArchiveFile IReadOnlyArchiveFileCollection.GetFile(string key)
		{
			return GetFile(key);
		}

		IArchiveFile IArchiveFileCollection.GetFile(string key)
		{
			return GetFile(key);
		}

		IArchiveFile IArchiveFileCollection.GetOrCreateFile(string key)
		{
			return GetOrCreateFile(key);
		}
	}
}
