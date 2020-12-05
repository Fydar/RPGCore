using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace RPGCore.FileTree.FileSystem
{
	public class FileSystemArchiveDirectoryCollection : IArchiveDirectoryCollection
	{
		private readonly FileSystemArchive archive;
		private readonly FileSystemArchiveDirectory owner;
		internal readonly List<FileSystemArchiveDirectory> internalList;
		public IEnumerable<FileSystemArchiveDirectory> All => internalList;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IEnumerable<IArchiveDirectory> IArchiveDirectoryCollection.All => internalList;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IEnumerable<IReadOnlyArchiveDirectory> IReadOnlyArchiveDirectoryCollection.All => internalList;


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
			for (int i = 0; i < internalList.Count; i++)
			{
				var directory = internalList[i];
				if (directory.Name == key)
				{
					return directory;
				}
			}
			return null;
		}

		public FileSystemArchiveDirectory GetOrCreateDirectory(string key)
		{
			var directory = GetDirectory(key);

			if (directory == null)
			{
				var directoryInfo = new DirectoryInfo(Path.Combine(owner.directoryInfo.FullName, key));
				directoryInfo.Create();
				directory = new FileSystemArchiveDirectory(archive, owner, directoryInfo);
			}
			return directory;
		}

		public IEnumerator<FileSystemArchiveDirectory> GetEnumerator()
		{
			return internalList.GetEnumerator();
		}

		IReadOnlyArchiveDirectory IReadOnlyArchiveDirectoryCollection.GetDirectory(string key)
		{
			return GetDirectory(key);
		}

		IArchiveDirectory IArchiveDirectoryCollection.GetDirectory(string key)
		{
			return GetDirectory(key);
		}

		IArchiveDirectory IArchiveDirectoryCollection.GetOrCreateDirectory(string key)
		{
			return GetOrCreateDirectory(key);
		}
	}
}
