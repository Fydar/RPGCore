using UnityEngine;
using RPGCore.Behaviour.Packages;

public static class IPackageAssetExtensions
{
	public static Texture2D LoadImage(this PackageResource asset)
	{
		var loadedImage = new Texture2D(2, 2);

		loadedImage.LoadImage(asset.LoadData(), true);

		return loadedImage;
	}
}
