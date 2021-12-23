using RPGCore.Packages;
using UnityEngine;
using UnityEngine.UI;

namespace RPGCoreUnity.Demo.Browser
{
	public class AssetRenderer : MonoBehaviour
	{
		public Text Name;
		public RawImage Icon;

		public void Render(IResource resource)
		{
			Name.text = resource.Name;
			if (resource.Name.EndsWith(".png"))
			{
				Icon.texture = resource.LoadImage();
			}
		}
	}
}
