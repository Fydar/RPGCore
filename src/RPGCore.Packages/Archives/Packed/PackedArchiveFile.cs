using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace RPGCore.Packages.Archives
{
	public class PackedArchiveFile : IArchiveFile
	{
		public PackedArchiveDirectory Parent { get; }

		public PackedArchive Archive { get; }

		public string Name { get; }
		public string FullName { get; }
		public string Extension { get; }

		public long CompressedSize => GetEntry().CompressedLength;
		public long UncompressedSize => GetEntry().Length;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IArchive IArchiveEntry.Archive => Archive;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IArchiveDirectory IArchiveEntry.Parent => Parent;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IReadOnlyArchive IReadOnlyArchiveEntry.Archive => Archive;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IReadOnlyArchiveDirectory IReadOnlyArchiveEntry.Parent => Parent;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] string IReadOnlyArchiveEntry.Name => Name;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] string IReadOnlyArchiveEntry.FullName => FullName;

		public PackedArchiveFile(PackedArchive archive, PackedArchiveDirectory parent, string name)
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

		public Task DeleteAsync()
		{
			return Task.Run(() => GetEntry()?.Delete());
		}

		public Stream OpenRead()
		{
			return GetEntry().Open();
		}

		public Stream OpenWrite()
		{
			return GetOrCreateEntry().Open();
		}

		public override string ToString()
		{
			return FullName;
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

		public Task MoveInto(IArchiveDirectory destination, string name)
		{
			throw new System.NotImplementedException();
		}

		public Task CopyInto(IArchiveDirectory destination, string name)
		{
			throw new System.NotImplementedException();
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
	}
}
