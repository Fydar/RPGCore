using RPGCore.Behaviour.Editor;
using UnityEditor;
using UnityEngine;

namespace RPGCore.Unity.Editors
{
	public class BehaviourEditor : EditorWindow
	{
		private BehaviourGraphFrame GraphFrame;

		[MenuItem("Window/Behaviour")]
		public static void Open()
		{
			var window = GetWindow<BehaviourEditor>();

			window.Show();
		}

		public static void Open(EditorSession session, EditorField graphField)
		{
			var window = GetWindow<BehaviourEditor>();

			window.Show();

			window.GraphFrame = new BehaviourGraphFrame();
			window.GraphFrame.View = new BehaviourEditorView();
			window.GraphFrame.View.BeginSession(session, graphField);

			window.GraphFrame.Window = window;
			window.GraphFrame.OnEnable();
		}

		private void OnEnable()
		{
			if (EditorGUIUtility.isProSkin)
			{
				titleContent = new GUIContent("Behaviour", BehaviourGraphResources.Instance.DarkThemeIcon);
			}
			else
			{
				titleContent = new GUIContent("Behaviour", BehaviourGraphResources.Instance.LightThemeIcon);
			}

			if (GraphFrame == null)
			{
				GraphFrame = new BehaviourGraphFrame();
			}
			GraphFrame.Window = this;
			GraphFrame.OnEnable();
		}

		private void OnGUI()
		{
			GraphFrame.Position = position;

			GraphFrame.OnGUI();
		}
	}
}
