using RPGCore.Packages.Archives;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace RPGCore.Packages
{
	[DebuggerDisplay("{ToString(),nq}")]
	public sealed class ProjectResourceContent : IResourceContent
	{
		public long UncompressedSize { get; }
		public IArchiveEntry ArchiveEntry { get; }

		internal ProjectResourceContent(IArchiveEntry archiveEntry)
		{
			ArchiveEntry = archiveEntry;
			UncompressedSize = ArchiveEntry.UncompressedSize;
		}

		public byte[] LoadData()
		{
			return File.ReadAllBytes(ArchiveEntry.FullName);
		}

		public async Task<byte[]> LoadDataAsync()
		{
			byte[] result;
			using (var stream = File.Open(ArchiveEntry.FullName, FileMode.Open))
			{
				result = new byte[stream.Length];
				await stream.ReadAsync(result, 0, (int)stream.Length);
			}
			return result;
		}

		public Stream LoadStream()
		{
			return ArchiveEntry.OpenRead();
		}

		public StreamWriter WriteStream()
		{
			return new StreamWriter(ArchiveEntry.FullName, false);
		}

		public override string ToString()
		{
			return new MemorySize(UncompressedSize).ToString();
		}
	}
}
