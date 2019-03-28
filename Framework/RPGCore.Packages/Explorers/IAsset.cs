namespace RPGCore.Packages
{
	public interface IAsset
	{
		string Name { get; }

		IResource GetResource (string path);
	}
}
