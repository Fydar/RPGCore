using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace RPGCore.Packages
{
	[DebuggerDisplay("{ToString(),nq}")]
	public sealed class ProjectResourceContent : IResourceContent
	{
		public long UncompressedSize { get; }
		public FileInfo FileInfo { get; }

		internal ProjectResourceContent(FileInfo fileInfo)
		{
			FileInfo = fileInfo;
			UncompressedSize = FileInfo.Length;
		}

		public byte[] LoadData()
		{
			return File.ReadAllBytes(FileInfo.FullName);
		}

		public async Task<byte[]> LoadDataAsync()
		{
			byte[] result;
			using (var stream = File.Open(FileInfo.FullName, FileMode.Open))
			{
				result = new byte[stream.Length];
				await stream.ReadAsync(result, 0, (int)stream.Length);
			}
			return result;
		}

		public Stream LoadStream()
		{
			return File.Open(FileInfo.FullName, FileMode.Open);
		}

		public StreamWriter WriteStream()
		{
			return new StreamWriter(FileInfo.FullName, false);
		}

		public override string ToString()
		{
			return new MemorySize(UncompressedSize).ToString();
		}
	}
}
