using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace RPGCore.FileTree.Packed
{
	public class PackedArchiveFile : IArchiveFile
	{
		public PackedArchiveDirectory Parent { get; }

		public PackedArchive Archive { get; }

		/// <inheritdoc/>
		public string Name { get; }

		/// <inheritdoc/>
		public string FullName { get; }

		/// <inheritdoc/>
		public string Extension { get; }

		/// <inheritdoc/>
		public long CompressedSize => GetEntry().CompressedLength;

		/// <inheritdoc/>
		public long UncompressedSize => GetEntry().Length;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IArchive IArchiveEntry.Archive => Archive;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IArchiveDirectory IArchiveEntry.Parent => Parent;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IReadOnlyArchive IReadOnlyArchiveEntry.Archive => Archive;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IReadOnlyArchiveDirectory IReadOnlyArchiveEntry.Parent => Parent;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] string IReadOnlyArchiveEntry.Name => Name;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] string IReadOnlyArchiveEntry.FullName => FullName;

		internal PackedArchiveFile(PackedArchive archive, PackedArchiveDirectory parent, string name)
		{
			Archive = archive;
			Parent = parent;

			Name = name;
			FullName = MakeFullName(parent, name);

			int dotIndex = name.LastIndexOf('.');
			Extension = dotIndex != -1
				? name.Substring(dotIndex)
				: "";
		}

		/// <inheritdoc/>
		public Task DeleteAsync()
		{
			return Task.Run(() => GetEntry()?.Delete());
		}

		/// <inheritdoc/>
		public Stream OpenRead()
		{
			return GetEntry().Open();
		}

		/// <inheritdoc/>
		public Stream OpenWrite()
		{
			return GetOrCreateEntry().Open();
		}

		/// <inheritdoc/>
		public Task MoveInto(IArchiveDirectory destination, string name)
		{
			throw new System.NotImplementedException();
		}

		/// <inheritdoc/>
		public Task CopyIntoAsync(IArchiveDirectory destination, string name)
		{
			throw new System.NotImplementedException();
		}

		private ZipArchiveEntry GetEntry()
		{
			if (Archive.ZipArchive.Mode == ZipArchiveMode.Create)
			{
				return null;
			}

			return Archive.ZipArchive.GetEntry(FullName);
		}

		private ZipArchiveEntry GetOrCreateEntry()
		{
			return GetEntry() ?? Archive.ZipArchive.CreateEntry(FullName);
		}

		private static string MakeFullName(IArchiveDirectory parent, string key)
		{
			if (parent == null || string.IsNullOrEmpty(parent.FullName))
			{
				return key;
			}
			else
			{
				return $"{parent.FullName}/{key}";
			}
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return FullName;
		}
	}
}
