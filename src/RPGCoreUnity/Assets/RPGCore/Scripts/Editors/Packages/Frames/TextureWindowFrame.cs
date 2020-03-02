using RPGCore.Packages;
using UnityEngine;

namespace RPGCore.Unity.Editors
{
	public class TextureWindowFrame : WindowFrame
	{
		private readonly Texture2D texture;

		public TextureWindowFrame(IResource resource)
		{
			texture = new Texture2D(2, 2);
			texture.LoadImage(resource.LoadData());
		}

		public override void OnEnable()
		{

		}

		public override void OnGUI()
		{
			int aspect = texture.width / texture.height;
			var rect = GUILayoutUtility.GetRect(480, 480 / aspect, GUILayout.ExpandWidth(false));

			GUI.DrawTexture(rect, texture);
		}
	}
}
