using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Packages
{
	public class ProjectDirectory : IDirectory
	{
		public string Name { get; }

		public string FullName { get; }

		public IReadOnlyList<IDirectory> Directories => directories;

		public IResourceCollection Resources => resources;

		public IDirectory Parent { get; private set; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly List<IDirectory> directories;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly ProjectResourceCollection resources;

		internal ProjectDirectory(string fullName)
		{
			FullName = fullName;
			Name = NameFromFullName(fullName);

			directories = new List<IDirectory>();
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
