using System.Diagnostics;

namespace RPGCore.Packages
{
	public class PackageDirectory : IDirectory
	{
		public string Name { get; }
		public string FullName { get; }

		public PackageDirectoryCollection Directories { get; }
		public PackageResourceCollection Resources { get; }
		public IDirectory Parent { get; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IDirectoryCollection IDirectory.Directories => Directories;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IResourceCollection IDirectory.Resources => Resources;

		internal PackageDirectory(string name, PackageDirectory parent)
		{
			Name = name;
			FullName = MakeFullName(parent, name);
			Parent = parent;

			Directories = new PackageDirectoryCollection();
			Resources = new PackageResourceCollection();
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
}
