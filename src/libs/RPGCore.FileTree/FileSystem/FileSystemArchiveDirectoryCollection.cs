using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace RPGCore.FileTree.FileSystem
{
	[DebuggerDisplay("Count = {Count,nq}")]
	public class FileSystemArchiveDirectoryCollection : IArchiveDirectoryCollection
	{
		private readonly FileSystemArchive archive;
		private readonly FileSystemArchiveDirectory owner;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly List<FileSystemArchiveDirectory> internalList;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly SemaphoreSlim synchronize;

		public IEnumerable<FileSystemArchiveDirectory> All => internalList;

		internal int Count
		{
			get
			{
				return internalList.Count;
			}
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IEnumerable<IArchiveDirectory> IArchiveDirectoryCollection.All => internalList;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IEnumerable<IReadOnlyArchiveDirectory> IReadOnlyArchiveDirectoryCollection.All => internalList;

		public FileSystemArchiveDirectoryCollection(FileSystemArchive archive, FileSystemArchiveDirectory owner)
		{
			this.archive = archive;
			this.owner = owner;

			synchronize = new SemaphoreSlim(1, 1);
			internalList = new List<FileSystemArchiveDirectory>();
			foreach (var fileInfo in owner.directoryInfo
				.EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
			{
				internalList.Add(new FileSystemArchiveDirectory(archive, owner, fileInfo));
			}
		}

		public FileSystemArchiveDirectory GetDirectory(string key)
		{
			synchronize.Wait();
			var result = GetDirectoryInternal(key);
			synchronize.Release();
			return result;
		}

		public FileSystemArchiveDirectory GetOrCreateDirectory(string key)
		{
			synchronize.Wait();
			var directory = GetDirectoryInternal(key);

			if (directory == null)
			{
				var directoryInfo = new DirectoryInfo(Path.Combine(owner.directoryInfo.FullName, key));
				directoryInfo.Create();
				directory = new FileSystemArchiveDirectory(archive, owner, directoryInfo);
			}
			synchronize.Release();
			return directory;
		}

		internal void TrackDirectoryInternal(FileSystemArchiveDirectory directory)
		{
			synchronize.Wait();
			internalList.Add(directory);
			synchronize.Release();
		}

		internal void UntrackDirectoryInternal(FileSystemArchiveDirectory directory)
		{
			synchronize.Wait();
			internalList.Remove(directory);
			synchronize.Release();
		}

		private FileSystemArchiveDirectory GetDirectoryInternal(string key)
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
