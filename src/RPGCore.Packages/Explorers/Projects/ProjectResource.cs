using System.IO;
using System.Threading.Tasks;

namespace RPGCore.Packages
{
	public sealed class ProjectResource : IResource
	{
		public readonly FileInfo Entry;

		public long CompressedSize { get; }
		public long UncompressedSize { get; }

		public string Name { get; }

		public string FullName { get; }

		public ProjectResourceDescription Description { get; }
		public ProjectExplorer Explorer { get; }

		IResourceDescription IResource.Description => Description;

		public ProjectResource(ProjectExplorer projectExplorer, string projectKey, FileInfo entry)
		{
			Entry = entry;

			UncompressedSize = entry.Length;
			CompressedSize = UncompressedSize;

			Name = Entry.Name;
			Explorer = projectExplorer;
			FullName = projectKey;

			Description = new ProjectResourceDescription(this);
		}

		public byte[] LoadData()
		{
			return File.ReadAllBytes(Entry.FullName);
		}

		public async Task<byte[]> LoadDataAsync()
		{
			byte[] result;
			using (var stream = File.Open(Entry.FullName, FileMode.Open))
			{
				result = new byte[stream.Length];
				await stream.ReadAsync(result, 0, (int)stream.Length);
			}
			return result;
		}

		public Stream LoadStream()
		{
			return File.Open(Entry.FullName, FileMode.Open);
		}

		public StreamWriter WriteStream()
		{
			return new StreamWriter(Entry.FullName, false);
		}

		public override string ToString()
		{
			return FullName;
		}
	}
}
