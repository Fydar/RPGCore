using System.Diagnostics;

namespace RPGCore.Packages
{
	public sealed class ProjectResource : IResource
	{
		public string Name { get; }
		public string FullName { get; }
		public string Extension { get; }

		public ProjectExplorer Explorer { get; }
		public ProjectResourceContent Content { get; internal set; }
		public ProjectDirectory Directory { get; }
		public ProjectResourceDependencies Dependencies { get; }
		public ProjectResourceDependencies Dependants { get; }
		public ProjectResourceTags Tags { get; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IExplorer IResource.Explorer => Explorer;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IResourceContent IResource.Content => Content;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IDirectory IResource.Directory => Directory;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IResourceDependencyCollection IResource.Dependencies => Dependencies;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IResourceDependencyCollection IResource.Dependants => Dependants;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IResourceTags IResource.Tags => Tags;

		internal ProjectResource(ProjectExplorer explorer, ProjectDirectory directory, string fullName)
		{
			Explorer = explorer;
			Directory = directory;

			FullName = fullName;
			Name = MakeName(fullName);
			Extension = fullName.Substring(fullName.LastIndexOf('.'));

			Tags = new ProjectResourceTags();
			Dependencies = new ProjectResourceDependencies(Explorer);
			Dependants = new ProjectResourceDependencies(Explorer);
		}

		public override string ToString()
		{
			return FullName;
		}

		private string MakeName(string fullname)
		{
			int pathIndex = fullname.LastIndexOf('/');

			if (pathIndex == -1)
			{
				return fullname;
			}
			else
			{
				return fullname.Substring(pathIndex + 1);
			}
		}
	}
}
