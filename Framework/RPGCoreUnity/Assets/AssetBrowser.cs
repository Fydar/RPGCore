using RPGCore.Behaviour.Packages;
using RPGCore.UI;
using UnityEngine;

public class AssetBrowser : MonoBehaviour
{
	public RectTransform Holder;
	public RawImagePool Assets;

	private void Start()
	{
		var explorer = ContentDatabase.PrimaryPackage;

		foreach (var asset in explorer.Folders)
		{
			var assetRenderer = Assets.Grab (Holder);
			Debug.Log(asset);
			assetRenderer.texture = FindIcon (asset).LoadImage ();
		}
	}

	public static PackageResource FindIcon(PackageAsset asset)
	{
		for (int i = 0; i < asset.Assets.Length; i++)
		{
			if (asset.Assets[i].Name.EndsWith (".png", System.StringComparison.Ordinal))
			{
				return asset.Assets[i];
			}
		}
		return default (PackageResource);
	}
}
