using System.Collections;
using System.Collections.Generic;

namespace RPGCore.FileTree.Packed
{
	public class PackedArchiveFileCollection : IArchiveFileCollection
	{
		private readonly PackedArchive archive;
		private readonly PackedArchiveDirectory owner;
		internal readonly List<PackedArchiveFile> internalList;

		internal PackedArchiveFileCollection(PackedArchive archive, PackedArchiveDirectory owner)
		{
			this.archive = archive;
			this.owner = owner;

			internalList = new List<PackedArchiveFile>();
		}

		/// <inheritdoc/>
		public PackedArchiveFile GetFile(string key)
		{
			foreach (var file in internalList)
			{
				if (file.Name == key)
				{
					return file;
				}
			}

			return null;
		}

		/// <inheritdoc/>
		public PackedArchiveFile GetOrCreateFile(string key)
		{
			foreach (var file in internalList)
			{
				if (file.Name == key)
				{
					return file;
				}
			}

			var newFile = new PackedArchiveFile(archive, owner, key);
			internalList.Add(newFile);
			return newFile;
		}

		public IEnumerator<PackedArchiveFile> GetEnumerator()
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
