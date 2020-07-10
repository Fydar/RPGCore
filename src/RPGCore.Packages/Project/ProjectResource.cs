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
		public FileInfo FileInfo { get; }

		public ProjectExplorer Explorer { get; }
		public ProjectDirectory Directory { get; internal set; }
		public ProjectResourceDependencies Dependencies { get; }
		public ProjectResourceTags Tags { get; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IExplorer IResource.Explorer => Explorer;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IDirectory IResource.Directory => Directory;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IResourceDependencies IResource.Dependencies => Dependencies;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IResourceTags IResource.Tags => Tags;

		internal ProjectResource(ProjectResourceImporter projectResourceImporter)
		{
			Explorer = projectResourceImporter.ProjectExplorer;
			Dependencies = new ProjectResourceDependencies(projectResourceImporter);
			Tags = new ProjectResourceTags(projectResourceImporter);

			FileInfo = projectResourceImporter.FileInfo;
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
