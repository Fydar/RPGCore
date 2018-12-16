using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RPGCore.UI.CursorManagement
{
	[CreateAssetMenu (menuName = "Cursor Style")]
	public class CursorStyle : ScriptableObject
	{
		public Vector2 Hotspot = Vector2.zero;

		public Texture2D Graphic;

#if UNITY_EDITOR
		[CustomEditor (typeof (CursorStyle))]
		class CursorStyleEditor : Editor
		{
			public override bool RequiresConstantRepaint ()
			{
				return true;
			}

			public override void OnInspectorGUI ()
			{
				DrawDefaultInspector ();

				Rect rect = GUILayoutUtility.GetRect (0, 64);
				GUI.Box (rect, GUIContent.none, EditorStyles.helpBox);


				CursorStyle style = (CursorStyle)target;

				if (rect.Contains (Event.current.mousePosition))
				{
					if (CursorManager.CurrentStyle != style)
						CursorManager.SetCursor (style);
				}
				else
				{
					if (CursorManager.CurrentStyle != CursorManager.GetStyle ("Default"))
						CursorManager.SetCursor (CursorManager.GetStyle ("Default"));
				}

				EditorGUIUtility.AddCursorRect (rect, MouseCursor.CustomCursor);
			}

			public override Texture2D RenderStaticPreview (string assetPath, Object[] subAssets, int width, int height)
			{
				CursorStyle style = (CursorStyle)target;

				return Instantiate(style.Graphic);
			}
		}
#endif
	}
}
