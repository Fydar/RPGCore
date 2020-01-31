using RPGCore.Packages;
using UnityEngine;

public static class IResourceExtensions
{
	public static Texture2D LoadImage(this IResource resource)
	{
		var loadedImage = new Texture2D(2, 2)
		{
			name = resource.Name
		};

		loadedImage.LoadImage(resource.LoadData(), true);

		return loadedImage;
	}
}
