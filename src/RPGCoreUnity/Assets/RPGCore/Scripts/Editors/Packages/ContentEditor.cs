using RPGCore.Behaviour.Editor;
using UnityEditor;
using UnityEngine;

namespace RPGCore.Unity.Editors
{
	public class ContentEditor : EditorWindow
	{
		private ContentEditorFrame Frame;

		[MenuItem("Window/Content")]
		public static void Open()
		{
			var window = GetWindow<ContentEditor>();

			window.Show();
		}

		private void OnEnable()
		{
			if (EditorGUIUtility.isProSkin)
			{
				titleContent = new GUIContent("Content", ContentEditorResources.Instance.DarkThemeIcon);
			}
			else
			{
				titleContent = new GUIContent("Content", ContentEditorResources.Instance.LightThemeIcon);
			}

			Frame = new ContentEditorFrame();
			Frame.Window = this;
			Frame.OnEnable();
		}

		private void OnGUI()
		{
			Frame.Position = position;

			Frame.OnGUI();
		}
	}
}
