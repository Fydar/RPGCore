using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.FileTree.Packed
{
	public class PackedArchiveDirectoryCollection : IArchiveDirectoryCollection
	{
		private readonly PackedArchive archive;
		private readonly PackedArchiveDirectory owner;
		internal readonly List<PackedArchiveDirectory> internalList;

		public IEnumerable<PackedArchiveDirectory> All => internalList;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IEnumerable<IArchiveDirectory> IArchiveDirectoryCollection.All => internalList;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IEnumerable<IReadOnlyArchiveDirectory> IReadOnlyArchiveDirectoryCollection.All => internalList;

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
			return null;
		}

		public PackedArchiveDirectory GetOrCreateDirectory(string key)
		{
			var directory = GetDirectory(key);

			if (directory == null)
			{
				directory = new PackedArchiveDirectory(archive, owner, key);
				internalList.Add(directory);
			}
			return directory;
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
