using System.Collections;
using System.Collections.Generic;

namespace RPGCore.FileTree.Packed
{
	public class PackedArchiveDirectoryCollection : IArchiveDirectoryCollection
	{
		private readonly PackedArchive archive;
		private readonly PackedArchiveDirectory owner;
		internal readonly List<PackedArchiveDirectory> internalList;

		public PackedArchiveDirectoryCollection(PackedArchive archive, PackedArchiveDirectory owner)
		{
			this.archive = archive;
			this.owner = owner;

			internalList = new List<PackedArchiveDirectory>();
		}

		public PackedArchiveDirectory GetDirectory(string key)
		{
			foreach (var directory in internalList)
			{
				if (directory.Name == key)
				{
					return directory;
				}
			}

			var newDirectory = new PackedArchiveDirectory(archive, owner, key);
			internalList.Add(newDirectory);
			return newDirectory;
		}

		public IEnumerator<PackedArchiveDirectory> GetEnumerator()
		{
			return internalList.GetEnumerator();
		}

		IEnumerator<IReadOnlyArchiveDirectory> IEnumerable<IReadOnlyArchiveDirectory>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator<IArchiveDirectory> IArchiveDirectoryCollection.GetEnumerator()
		{
			return internalList.GetEnumerator();
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
