using RPGCore.Packages.Pipeline;
using System.Diagnostics;

namespace RPGCore.Packages
{
	public sealed class ProjectResource : IResource
	{
		public string Name { get; }
		public string FullName { get; }
		public string Extension { get; }

		public long UncompressedSize { get; }

		public ProjectExplorer Explorer { get; }
		public ProjectResourceContent Content { get; internal set; }
		public ProjectDirectory Directory { get; }
		public ProjectResourceDependencies Dependencies { get; }
		public ProjectResourceTags Tags { get; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IExplorer IResource.Explorer => Explorer;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IResourceContent IResource.Content => Content;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IDirectory IResource.Directory => Directory;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IResourceDependencies IResource.Dependencies => Dependencies;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IResourceTags IResource.Tags => Tags;

		internal ProjectResource(ProjectDirectory directory, ProjectResourceImporter projectResourceImporter)
		{
			Explorer = projectResourceImporter.ProjectExplorer;
			Content = new ProjectResourceContent(projectResourceImporter.ArchiveEntry);
			Directory = directory;
			Dependencies = new ProjectResourceDependencies(projectResourceImporter);
			Tags = new ProjectResourceTags(projectResourceImporter);

			FullName = projectResourceImporter.ProjectKey;

			Name = projectResourceImporter.ArchiveEntry.Name;
			Extension = projectResourceImporter.ArchiveEntry.Extension;
			UncompressedSize = projectResourceImporter.ArchiveEntry.UncompressedSize;
		}

		public override string ToString()
		{
			return FullName;
		}
	}
}
