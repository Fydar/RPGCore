using RPGCore.Packages;
using System.Diagnostics;

namespace RPGCore.Projects;

public class ProjectDirectory : IDirectory
{
	public string Name { get; }
	public string FullName { get; }
	public ProjectDirectoryCollection Directories { get; }
	public ProjectResourceCollection Resources { get; }
	public IDirectory Parent { get; }

	[DebuggerBrowsable(DebuggerBrowsableState.Never)] IDirectoryCollection IDirectory.Directories => Directories;
	[DebuggerBrowsable(DebuggerBrowsableState.Never)] IResourceCollection IDirectory.Resources => Resources;

	internal ProjectDirectory(ProjectDirectory parent, string name)
	{
		Parent = parent;
		Name = name;

		FullName = MakeFullName(parent, name);

		Directories = new ProjectDirectoryCollection();
		Resources = new ProjectResourceCollection();
	}

	private static string MakeFullName(IDirectory parent, string key)
	{
		if (parent == null || string.IsNullOrEmpty(parent.FullName))
		{
			return key;
		}
		else
		{
			return $"{parent.FullName}/{key}";
		}
	}
}
