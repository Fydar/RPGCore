using System.IO;
using System.Threading.Tasks;

namespace RPGCore.Packages
{
	public class ProjectResource : IResource
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
		

		public byte[] LoadData ()
		{
			return File.ReadAllBytes (Entry.FullName);
		}

		public async Task<byte[]> LoadDataAsync ()
		{
			byte[] result;
			using (var stream = File.Open (Entry.FullName, FileMode.Open))
			{
				result = new byte[stream.Length];
				await stream.ReadAsync (result, 0, (int)stream.Length);
			}
			return result;
		}

		public override string ToString ()
		{
			return Name;
		}

        public PackageStream LoadStream()
        {
            throw new System.NotImplementedException();
        }
    }
}
