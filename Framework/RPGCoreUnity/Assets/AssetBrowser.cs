using RPGCore.Packages;
using RPGCore.UI;
using RPGCore.Unity;
using UnityEngine;

public class AssetBrowser : MonoBehaviour
{
	public PackageImport Package;

	public RectTransform Holder;
	public AssetRendererPool Assets;

	private void Start()
	{
		var explorer = Package.Explorer;

		foreach (var asset in explorer.Folders)
		{
			var assetRenderer = Assets.Grab (Holder);
			assetRenderer.Render (asset);
		}
	}
}
