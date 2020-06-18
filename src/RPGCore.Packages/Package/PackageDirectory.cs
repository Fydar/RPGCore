namespace RPGCore.Packages
{
	public class PackageDirectory : IDirectory
	{
		public string Name { get; internal set; }

		public string FullName { get; internal set; }

		public IDirectoryCollection Directories { get; internal set; }

		public IResourceCollection Resources { get; internal set; }

		public IDirectory Parent { get; internal set; }

		internal PackageDirectory()
		{

		}
	}
}
