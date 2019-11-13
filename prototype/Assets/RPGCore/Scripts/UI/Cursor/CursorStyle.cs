﻿using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RPGCore.UI.CursorManagement
{
	[CreateAssetMenu(menuName = "Cursor Style")]
	public class CursorStyle : ScriptableObject
	{
		public Vector2 Hotspot = Vector2.zero;

		public Texture2D Graphic;

#if UNITY_EDITOR
		[CustomEditor(typeof(CursorStyle))]
		private class CursorStyleEditor : Editor
		{
			public override bool RequiresConstantRepaint()
			{
				return true;
			}

			public override void OnInspectorGUI()
			{
				var style = (CursorStyle)target;
				DrawDefaultInspector();

				GUILayout.Space(20);

				var pointRow = GUILayoutUtility.GetRect(0, 160);
				var pointRect = new Rect(pointRow)
				{
					x = pointRow.x + (pointRow.width * 0.5f) - (pointRow.height * 0.5f),
					width = pointRow.height,
				};
				GUI.Box(pointRect, GUIContent.none, EditorStyles.helpBox);

				GUI.DrawTexture(pointRect, style.Graphic);
				if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag)
				{
					if (pointRect.Contains(Event.current.mousePosition))
					{
						style.Hotspot = new Vector2(
							Mathf.InverseLerp(pointRect.xMin, pointRect.xMax, Event.current.mousePosition.x) * style.Graphic.width,
							Mathf.InverseLerp(pointRect.yMin, pointRect.yMax, Event.current.mousePosition.y) * style.Graphic.height
						);
					}
				}

				var pointerPoint = new Vector2(
					Mathf.Lerp(pointRect.xMin, pointRect.xMax, style.Hotspot.x / style.Graphic.width),
					Mathf.Lerp(pointRect.yMin, pointRect.yMax, style.Hotspot.y / style.Graphic.height));

				var horizontal = new Rect(pointRect.xMin, pointerPoint.y - 1, pointRect.width, 1);
				var vertical = new Rect(pointerPoint.x - 1, pointRect.yMin, 1, pointRect.width);

				EditorGUI.DrawRect(horizontal, Color.red);
				EditorGUI.DrawRect(vertical, Color.red);

				GUILayout.Space(20);

				var rect = GUILayoutUtility.GetRect(0, 96);
				var previewRect = new Rect(rect)
				{
					x = rect.x + (rect.width * 0.5f) - (rect.height * 0.5f),
					width = rect.height,
				};
				GUI.Box(previewRect, GUIContent.none, EditorStyles.helpBox);

				if (previewRect.Contains(Event.current.mousePosition))
				{
					if (CursorManager.CurrentStyle != style)
					{
						CursorManager.SetCursor(style);
					}
				}
				else
				{
					if (CursorManager.CurrentStyle != CursorManager.GetStyle("Default"))
					{
						CursorManager.SetCursor(CursorManager.GetStyle("Default"));
					}
				}

				EditorGUIUtility.AddCursorRect(previewRect, MouseCursor.CustomCursor);
			}

			public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
			{
				var style = (CursorStyle)target;

				return Instantiate(style.Graphic);
			}
		}
#endif
	}
}
