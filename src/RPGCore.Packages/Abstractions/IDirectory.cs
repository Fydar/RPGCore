namespace RPGCore.Packages
{
	public interface IDirectory
	{
		string Name { get; }
		string FullName { get; }

		IDirectory Parent { get; }
		IDirectoryCollection Directories { get; }
		IResourceCollection Resources { get; }
	}
}
