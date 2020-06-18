using RPGCore.Packages.Pipeline;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace RPGCore.Packages
{
	public sealed class ProjectResource : IResource
	{
		public string Name { get; }
		public string FullName { get; }
		public string Extension { get; }

		public long UncompressedSize { get; }

		public ProjectResourceTags Tags { get; }
		public ProjectExplorer Explorer { get; }
		public IDirectory Directory { get; internal set; }

		public FileInfo FileInfo { get; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IResourceTags IResource.Tags => Tags;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IExplorer IResource.Explorer => Explorer;

		internal ProjectResource(ProjectResourceImporter projectResourceImporter)
		{
			Tags = new ProjectResourceTags(projectResourceImporter);

			FileInfo = projectResourceImporter.FileInfo;
			Explorer = projectResourceImporter.ProjectExplorer;
			FullName = projectResourceImporter.ProjectKey;

			Name = FileInfo.Name;
			Extension = FileInfo.Extension;
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
			return FullName;
		}
	}
}
