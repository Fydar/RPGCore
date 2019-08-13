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
	
	public void Render(IResource resource)
	{
		Name.text = resource.Name;
		if (resource.Name.EndsWith(".png"))
		{
			Icon.texture = resource.LoadImage();
		}
	}
}
