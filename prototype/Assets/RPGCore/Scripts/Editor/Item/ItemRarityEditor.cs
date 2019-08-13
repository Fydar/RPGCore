#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace RPGCore.Utility.Editors
{
	[CustomEditor (typeof (ItemRarity))]
	[CanEditMultipleObjects]
	public class ItemRarityEditor : Editor
	{
		public override Texture2D RenderStaticPreview (string assetPath, Object[] subAssets, int width, int height)
		{
			var rarity = (ItemRarity)target;

			var colorA = rarity.HeaderText;
			var colorB = rarity.HeaderBackground;

			if ((colorA.r + colorA.g + colorA.b) * 0.33f < (colorB.r + colorB.g + colorB.b) * 0.33f)
			{
				var temp = colorA;
				colorA = colorB;
				colorB = temp;
			}

			return RenderShader (colorA, colorB, width, height);
		}

		public Texture2D RenderShader (Color primary, Color secondary, int width, int height)
		{
			var preview = new Texture2D (width, height);

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					if (x > y)
						preview.SetPixel (x, y, primary);
					else if (x == y)
						preview.SetPixel (x, y, new Color (
							(primary.r + secondary.r) * 0.5f,
							(primary.g + secondary.g) * 0.5f,
							(primary.b + secondary.b) * 0.5f));
					else
						preview.SetPixel (x, y, secondary);
				}
			}
			preview.Apply ();
			return preview;
		}
	}
}
#endif
