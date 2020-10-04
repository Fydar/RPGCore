using System.Collections;
using System.Collections.Generic;

namespace RPGCore.Packages.Archives
{
	public class PackedArchiveFileCollection : IArchiveFileCollection
	{
		private readonly PackedArchive archive;
		private readonly PackedArchiveDirectory owner;
		internal readonly List<PackedArchiveFile> files;

		public PackedArchiveFileCollection(PackedArchive archive, PackedArchiveDirectory owner)
		{
			this.archive = archive;
			this.owner = owner;

			files = new List<PackedArchiveFile>();
		}

		public PackedArchiveFile GetOrCreateFile(string key)
		{
			foreach (var file in files)
			{
				if (file.Name == key)
				{
					return file;
				}
			}

			var newFile = new PackedArchiveFile(archive, owner, key);
			files.Add(newFile);
			return newFile;
		}

		public IEnumerator<PackedArchiveFile> GetEnumerator()
		{
			return files.GetEnumerator();
		}

		IEnumerator<IReadOnlyArchiveFile> IEnumerable<IReadOnlyArchiveFile>.GetEnumerator() => GetEnumerator();
		IEnumerator<IArchiveFile> IArchiveFileCollection.GetEnumerator() => GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		IReadOnlyArchiveFile IReadOnlyArchiveFileCollection.GetFile(string key) => GetOrCreateFile(key);
		IArchiveFile IArchiveFileCollection.GetFile(string key) => GetOrCreateFile(key);
	}
}
