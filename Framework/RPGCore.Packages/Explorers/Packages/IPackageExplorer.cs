namespace RPGCore.Packages
{
	public interface IPackageExplorer
	{
		string Name { get; }
		string Version { get; }
		PackageDependancy[] Dependancies { get; }
		IPackageAssetCollection Folders { get; }
	}
}