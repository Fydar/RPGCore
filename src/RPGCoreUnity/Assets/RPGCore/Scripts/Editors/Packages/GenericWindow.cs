using UnityEditor;
using UnityEngine;

namespace RPGCore.Unity.Editors
{
	public class GenericWindow : EditorWindow
	{
		public GUIContent WindowTitle;
		private WindowFrame Frame;

		public static void Open(GUIContent title, WindowFrame frame)
		{
			var window = CreateWindow<GenericWindow>();

			window.Show();

			window.WindowTitle = title;
			window.titleContent = title;

			window.Frame = frame;
			window.Frame.Window = window;
			window.Frame.OnEnable();
		}

		private void OnEnable()
		{
			if (Frame != null)
			{
				Frame.Window = this;
				Frame.OnEnable();
			}
			if (WindowTitle != null)
			{
				titleContent = WindowTitle;
			}
		}

		private void OnGUI()
		{
			if (Frame == null)
			{
				Close();
				return;
			}

			Frame.OnGUI();
		}
	}
}
