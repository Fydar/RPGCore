using System.Collections;
using System.Collections.Generic;

namespace RPGCore.Packages.Archives
{
	public class PackedArchiveDirectoryCollection : IArchiveDirectoryCollection
	{
		private readonly PackedArchive archive;
		private readonly PackedArchiveDirectory owner;
		internal readonly List<PackedArchiveDirectory> directories;

		public PackedArchiveDirectoryCollection(PackedArchive archive, PackedArchiveDirectory owner)
		{
			this.archive = archive;
			this.owner = owner;

			directories = new List<PackedArchiveDirectory>();
		}

		public PackedArchiveDirectory GetDirectory(string key)
		{
			foreach (var directory in directories)
			{
				if (directory.Name == key)
				{
					return directory;
				}
			}

			var newDirectory = new PackedArchiveDirectory(archive, owner, key);
			directories.Add(newDirectory);
			return newDirectory;
		}

		public IEnumerator<PackedArchiveDirectory> GetEnumerator()
		{
			return directories.GetEnumerator();
		}

		IEnumerator<IReadOnlyArchiveDirectory> IEnumerable<IReadOnlyArchiveDirectory>.GetEnumerator() => GetEnumerator();
		IEnumerator<IArchiveDirectory> IArchiveDirectoryCollection.GetEnumerator() => directories.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		IReadOnlyArchiveDirectory IReadOnlyArchiveDirectoryCollection.GetDirectory(string key) => GetDirectory(key);
		IArchiveDirectory IArchiveDirectoryCollection.GetDirectory(string key) => GetDirectory(key);

	}
}
