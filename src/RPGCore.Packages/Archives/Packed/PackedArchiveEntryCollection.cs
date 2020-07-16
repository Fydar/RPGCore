using System.Collections;
using System.Collections.Generic;

namespace RPGCore.Packages.Archives
{
	public class PackedArchiveEntryCollection : IArchiveEntryCollection
	{
		private readonly PackedArchive archive;

		public PackedArchiveEntryCollection(PackedArchive archive)
		{
			this.archive = archive;
		}

		public PackedArchiveEntry GetFile(string key)
		{
			return new PackedArchiveEntry(archive, key);
		}

		public IEnumerator<PackedArchiveEntry> GetEnumerator()
		{
			foreach (var zipArchiveEntry in archive.ZipArchive.Entries)
			{
				yield return new PackedArchiveEntry(archive, zipArchiveEntry.FullName);
			}
		}

		IEnumerator<IReadOnlyArchiveEntry> IEnumerable<IReadOnlyArchiveEntry>.GetEnumerator() => GetEnumerator();
		IEnumerator<IArchiveEntry> IEnumerable<IArchiveEntry>.GetEnumerator() => GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		IReadOnlyArchiveEntry IReadOnlyArchiveEntryCollection.GetFile(string key) => GetFile(key);
		IArchiveEntry IArchiveEntryCollection.GetFile(string key) => GetFile(key);
	}
}
