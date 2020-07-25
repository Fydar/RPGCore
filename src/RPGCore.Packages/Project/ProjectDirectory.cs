using System.Diagnostics;

namespace RPGCore.Packages
{
	public class ProjectDirectory : IDirectory
	{
		public string Name { get; }
		public string FullName { get; }

		public ProjectDirectoryCollection Directories { get; }
		public ProjectResourceCollection Resources { get; }
		public IDirectory Parent { get; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IDirectoryCollection IDirectory.Directories => Directories;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IResourceCollection IDirectory.Resources => Resources;

		internal ProjectDirectory(string name, string fullName, ProjectDirectory parent)
		{
			Name = name;
			FullName = fullName;
			Parent = parent;

			Directories = new ProjectDirectoryCollection();
			Resources = new ProjectResourceCollection();
		}
	}
}
