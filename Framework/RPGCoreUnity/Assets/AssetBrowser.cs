using RPGCore.Packages;
using RPGCore.UI;
using UnityEngine;

public class AssetBrowser : MonoBehaviour
{
	public RectTransform Holder;
	public AssetRendererPool Assets;

	private void Start()
	{
		var explorer = ContentDatabase.PrimaryPackage;

		foreach (var asset in explorer.Folders)
		{
			var assetRenderer = Assets.Grab (Holder);
			assetRenderer.Render (asset);
		}
	}
}
