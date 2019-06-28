using RPGCore.Packages;
using RPGCore.UI;
using RPGCore.Unity;
using UnityEngine;

public class AssetBrowser : MonoBehaviour
{
	public ProjectImport Package;

	public RectTransform Holder;
	public AssetRendererPool Assets;

	private void Start()
	{
		var explorer = Package.Explorer;

		foreach (var resource in explorer.Resources)
		{
			var assetRenderer = Assets.Grab (Holder);
			assetRenderer.Render (resource);
		}
	}
}
