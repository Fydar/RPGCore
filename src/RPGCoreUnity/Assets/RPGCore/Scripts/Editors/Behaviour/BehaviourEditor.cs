using RPGCore.Behaviour.Editor;
using RPGCore.DataEditor;
using UnityEditor;
using UnityEngine;

namespace RPGCoreUnity.Editors
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

		public static void Open(EditorSession session, EditorObject editorObject)
		{
			var window = GetWindow<BehaviourEditor>();

			window.Show();

			window.GraphFrame = new BehaviourGraphFrame
			{
				View = new BehaviourEditorView()
			};
			window.GraphFrame.View.BeginSession(session, editorObject);

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
			var graphFrame = new Rect(0, EditorGUIUtility.singleLineHeight + 1,
				position.width, position.height - (EditorGUIUtility.singleLineHeight + 1)); ;

			GraphFrame.Position = graphFrame;

			GraphFrame.OnGUI();

			DrawTopBar();
		}

		private void DrawTopBar()
		{
			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.ExpandWidth(true));

			if (GUILayout.Button(GraphFrame.View?.DescribeCurrentAction, EditorStyles.toolbarButton, GUILayout.Width(280)))
			{
			}

			EditorGUILayout.EndHorizontal();
		}
	}
}
