namespace RPGCore.Behaviour.Packages
{
	public interface IPackageExplorer
	{
		PackageDependancy[] Dependancies { get; }
		IPackageItemCollection Items { get; }
		string Name { get; }
		string Version { get; }
	}
}