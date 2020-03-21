using System.Diagnostics;
using System.IO;

namespace RPGCore.Packages
{
	public class ProjectDirectory : IDirectory
	{
		public string Name { get; }
		public string FullName { get; }
		public DirectoryInfo PhysicalLocation { get; }

		public IDirectory Parent { get; private set; }

		public IDirectoryCollection Directories => directories;
		public IResourceCollection Resources => resources;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly ProjectDirectoryCollection directories;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly ProjectResourceCollection resources;

		internal ProjectDirectory(DirectoryInfo physicalLocation, string fullName)
		{
			FullName = fullName;
			Name = NameFromFullName(fullName);
			PhysicalLocation = physicalLocation;

			directories = new ProjectDirectoryCollection();
			resources = new ProjectResourceCollection();
		}

		internal void AddChildDirectory(ProjectDirectory projectDirectory)
		{
			directories.Add(projectDirectory);
			projectDirectory.Parent = this;
		}

		internal void AddChildResource(ProjectResource projectResource)
		{
			resources.Add(projectResource);

			projectResource.Directory = this;
		}

		private static string NameFromFullName(string fullName)
		{
			int lastDelimiter = fullName.LastIndexOf('/');

			return lastDelimiter == -1
				? fullName
				: fullName.Substring(lastDelimiter + 1);
		}
	}
}
