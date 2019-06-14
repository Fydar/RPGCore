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

	public static IResource FindIcon (this IAsset asset)
	{
		foreach (var resource in asset.Resources)
		{
			if (resource.Name.EndsWith (".png", System.StringComparison.Ordinal))
			{
				return resource;
			}
		}
		return default (PackageResource);
	}
}
