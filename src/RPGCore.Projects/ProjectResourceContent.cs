using RPGCore.FileTree;
using RPGCore.Packages;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace RPGCore.Projects
{
	[DebuggerDisplay("{ToString(),nq}")]
	public sealed class ProjectResourceContent : IResourceContent
	{
		public long UncompressedSize { get; }
		public IArchiveFile ArchiveEntry { get; }

		internal ProjectResourceContent(IArchiveFile archiveEntry)
		{
			ArchiveEntry = archiveEntry;
			UncompressedSize = ArchiveEntry.UncompressedSize;
		}

		public byte[] LoadData()
		{
			using var stream = ArchiveEntry.OpenRead();
			byte[] buffer = new byte[ArchiveEntry.UncompressedSize];
			stream.Read(buffer, 0, buffer.Length);

			return buffer;
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

		public Stream OpenWrite()
		{
			return ArchiveEntry.OpenWrite();
		}

		public override string ToString()
		{
			return new MemorySize(UncompressedSize).ToString();
		}
	}
}
