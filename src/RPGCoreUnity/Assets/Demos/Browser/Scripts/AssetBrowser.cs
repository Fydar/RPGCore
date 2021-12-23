using RPGCoreUnity;
using UnityEngine;

namespace RPGCoreUnity.Demo.Browser
{
	public class AssetBrowser : MonoBehaviour
	{
		public ProjectImport Package;

		public RectTransform Holder;
		public AssetRendererPool Assets;

		private void Start()
		{
			var explorer = Package.SourceFiles;

			foreach (var resource in explorer.Resources)
			{
				var assetRenderer = Assets.Grab(Holder);
				assetRenderer.Render(resource);
			}
		}
	}
}
