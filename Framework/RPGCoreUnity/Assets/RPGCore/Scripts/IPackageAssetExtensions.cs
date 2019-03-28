using UnityEngine;
using RPGCore.Packages;

public static class IPackageAssetExtensions
{
	public static Texture2D LoadImage(this PackageResource resource)
	{
		var loadedImage = new Texture2D (2, 2)
		{
			name = resource.Name
		};

		loadedImage.LoadImage(resource.LoadData(), true);

		return loadedImage;
	}
}
