namespace RPGCore.Behaviour.Packages
{
	public interface IPackageItemCollection
	{
		PackageItem this[string key] { get; }
	}
}
