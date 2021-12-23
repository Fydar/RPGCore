using RPGCore.Packages;
using UnityEngine;

namespace RPGCoreUnity.Editors
{
	public class TextureWindowFrame : WindowFrame
	{
		private readonly Texture2D texture;

		public TextureWindowFrame(IResource resource)
		{
			texture = new Texture2D(2, 2);
			texture.LoadImage(resource.Content.LoadData());

			texture.filterMode = FilterMode.Point;
		}

		public override void OnEnable()
		{

		}

		public override void OnGUI()
		{
			GUILayout.BeginArea(Position);

			int aspect = texture.width / texture.height;
			var rect = GUILayoutUtility.GetRect(480, 480 / aspect, GUILayout.ExpandWidth(false));

			rect = new Rect(rect.x + 24,
				rect.y + 24,
				rect.width - 48,
				rect.height - 48);

			GUI.DrawTexture(rect, texture);

			GUILayout.EndArea();
		}
	}
}
