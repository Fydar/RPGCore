using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace RPGCore.Packages.Archives
{
	public class PackedArchiveEntry : IArchiveEntry
	{
		public PackedArchive Archive { get; }

		public string Name { get; }
		public string FullName { get; }
		public string Extension { get; }

		public long CompressedSize => GetEntry().CompressedLength;
		public long UncompressedSize => GetEntry().Length;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] string IReadOnlyArchiveEntry.Name => Name;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] string IReadOnlyArchiveEntry.FullName => FullName;

		public PackedArchiveEntry(PackedArchive archive, string key)
		{
			Archive = archive;
			FullName = key;

			int directoryIndex = key.LastIndexOf('/');
			Name = directoryIndex == -1
				? key
				: key.Substring(directoryIndex + 1);

			int dotIndex = key.LastIndexOf('.');
			Extension = dotIndex != -1
				? key.Substring(dotIndex)
				: "";
		}

		public Task DeleteAsync()
		{
			return Task.Run(() => GetEntry()?.Delete());
		}

		public async Task RenameAsync(string destination)
		{
			var entry = Archive.ZipArchive.GetEntry(FullName);

			await DuplicateAsync(destination);

			entry.Delete();
		}

		public async Task<PackedArchiveEntry> DuplicateAsync(string destination)
		{
			var destinationEntry = Archive.ZipArchive.CreateEntry(destination);

			using var destinationStream = destinationEntry.Open();
			using var sourceStream = GetEntry().Open();

			await sourceStream.CopyToAsync(destinationStream);

			return new PackedArchiveEntry(Archive, FullName);
		}

		public async Task UpdateAsync(Stream content)
		{
			using var fs = GetOrCreateEntry().Open();
			await content.CopyToAsync(fs);
		}

		async Task<IArchiveEntry> IArchiveEntry.DuplicateAsync(string destination)
		{
			var result = await DuplicateAsync(destination);
			return result;
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
	}
}
