using RPGCore.Packages;
using RPGCore.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable] public class AssetRendererPool : UIPool<AssetRenderer> { }

public class AssetRenderer : MonoBehaviour
{
	public Text Name;
	public RawImage Icon;
	
	public void Render(IAsset asset)
	{
		Name.text = asset.Name;
		Icon.texture = asset.FindIcon ().LoadImage();
	}
}
