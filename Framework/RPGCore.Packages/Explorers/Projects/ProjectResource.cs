using System.IO;
using System.Threading.Tasks;

namespace RPGCore.Packages
{
	public struct ProjectResource
	{
		public readonly FileInfo Entry;
		
		public long CompressedSize { get; }
		public long UncompressedSize { get; }

		public string Name
		{
			get
			{
				return Entry.Name;
			}
		}

		public ProjectResource (FileInfo entry)
		{
			Entry = entry;

			CompressedSize = 0;
			UncompressedSize = 0;
		}

		public override string ToString ()
		{
			return Name;
		}

		public byte[] LoadData()
		{
			return File.ReadAllBytes(Entry.FullName);
		}

		public Task<byte[]> LoadDataAsync()
		{
			string filePath = Entry.FullName;
			return Task.Run(() => File.ReadAllBytes(filePath));
		}
	}
}
