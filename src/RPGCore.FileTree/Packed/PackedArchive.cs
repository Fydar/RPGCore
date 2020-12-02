using System;
using System.Diagnostics;
using System.IO.Compression;

namespace RPGCore.FileTree.Packed
{
	public class PackedArchive : IArchive
	{
		public ZipArchive ZipArchive { get; }
		public PackedArchiveDirectory RootDirectory { get; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IArchiveDirectory IArchive.RootDirectory => RootDirectory;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IReadOnlyArchiveDirectory IReadOnlyArchive.RootDirectory => RootDirectory;

		public int MaximumWriteThreads => 1;

		public event Action<ArchiveEventParameters> OnEntryChanged;

		public PackedArchive(ZipArchive zipArchive)
		{
			ZipArchive = zipArchive;
			RootDirectory = new PackedArchiveDirectory(this, null, "");

			if (zipArchive.Mode != ZipArchiveMode.Create)
			{
				foreach (var zipEntry in zipArchive.Entries)
				{
					string[] elements = zipEntry.FullName
						.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

					var placeDirectory = RootDirectory;
					for (int i = 0; i < elements.Length - 1; i++)
					{
						string element = elements[i];

						placeDirectory = placeDirectory.Directories.GetOrCreateDirectory(element);
					}

					placeDirectory.Files.internalList.Add(new PackedArchiveFile(this, placeDirectory, zipEntry.Name));
				}
			}
		}
	}
}
