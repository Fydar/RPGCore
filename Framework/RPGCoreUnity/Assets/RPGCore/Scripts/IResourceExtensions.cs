using UnityEngine;
using RPGCore.Packages;

public static class IResourceExtensions
{
	public static Texture2D LoadImage(this IResource resource)
	{
		var loadedImage = new Texture2D (2, 2)
		{
			name = resource.Name
		};

		loadedImage.LoadImage(resource.LoadData(), true);

		return loadedImage;
	}

	public static PackageResource FindIcon (this PackageAsset asset)
	{
		for (int i = 0; i < asset.Resources.Length; i++)
		{
			if (asset.Resources[i].Name.EndsWith (".png", System.StringComparison.Ordinal))
			{
				return asset.Resources[i];
			}
		}
		return default (PackageResource);
	}
}
