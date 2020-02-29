using RPGCore.Packages.Pipeline;
using System.Diagnostics;
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

		public ProjectResourceTags Tags { get; }
		public ProjectExplorer Explorer { get; }
		public IDirectory Directory { get; internal set; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		IResourceTags IResource.Tags => Tags;

		internal ProjectResource(ProjectResourceImporter projectResourceImporter)
		{
			Entry = projectResourceImporter.FileInfo;

			Name = Entry.Name;
			UncompressedSize = Entry.Length;
			CompressedSize = UncompressedSize;

			Explorer = projectResourceImporter.ProjectExplorer;
			FullName = projectResourceImporter.ProjectKey;

			Tags = new ProjectResourceTags(projectResourceImporter);
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
