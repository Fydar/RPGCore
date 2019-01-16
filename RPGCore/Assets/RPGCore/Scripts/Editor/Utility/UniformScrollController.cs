#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPGCore.Editors.Utility
{
	public struct UniformScrollController : IEnumerable<ElementDrawer>, IEnumerator<ElementDrawer>
	{
		private static float ScrollSpeed = 6;
		private static float LockOffset = -10000f;

		private Rect marchingRect;
		private int currentIndex;

		private readonly float maxOverflow;
		private readonly int maxElementCount;
		private readonly float maxScroll;
		private readonly float viewScroll;
		private readonly int indexOffset;
		private readonly int elementCount;
		private readonly float localOffset;

		public UniformScrollController (Rect area, float height, ref Vector2 offset, int elements)
		{
			currentIndex = 0;

			maxOverflow = area.height % height;
			maxElementCount = Mathf.CeilToInt (area.height / height);
			maxScroll = (Mathf.Max (0.0f, (elements - maxElementCount) + 1) * height) - (maxOverflow);

			viewScroll = offset.y;
			if (offset.y < 0)
				viewScroll = maxScroll;

			localOffset = viewScroll % height;
			elementCount = Mathf.CeilToInt ((area.height + localOffset) / height);
			indexOffset = Mathf.Max (0, (int)(Mathf.Floor (viewScroll / height)));

			Rect verticalScrollRect = new Rect (area)
			{
				xMin = area.xMax - EditorGUIUtility.singleLineHeight
			};
			area.xMax = verticalScrollRect.xMin;

			if (maxScroll <= 0.0f)
			{
				EditorGUI.BeginDisabledGroup (true);
				GUI.VerticalScrollbar (verticalScrollRect, 0.0f, 1.0f, 0.0f, 1.0f);
				EditorGUI.EndDisabledGroup ();
			}
			else
			{
				if (Event.current.rawType == EventType.ScrollWheel && area.Contains (Event.current.mousePosition))
				{
					if (Event.current.delta.y > 0)
					{
						offset.y += Event.current.delta.y * ScrollSpeed;
						if (offset.y >= maxScroll - 0.075f || offset.y < 0.075f)
							offset.y = LockOffset;
					}
					else
					{
						if (offset.y <= LockOffset + 0.015f)
							offset.y = maxScroll;

						offset.y += Event.current.delta.y * ScrollSpeed;
						if (offset.y < 0 && offset.y > LockOffset + 1.0f)
							offset.y = 0;
					}
					Event.current.Use ();
				}

				float handleSize = area.height;

				EditorGUI.BeginChangeCheck ();
				float inputScroll = GUI.VerticalScrollbar (verticalScrollRect,
					viewScroll, handleSize, 0.0f, maxScroll + handleSize);
				if (EditorGUI.EndChangeCheck ())
				{
					if (inputScroll >= maxScroll - 0.015f)
						offset.y = LockOffset;
					else
						offset.y = inputScroll;
				}
			}

			marchingRect = new Rect (0, -localOffset, area.width, height);

			marchingRect.y -= marchingRect.height;
			currentIndex--;

			GUI.BeginGroup (area, GUI.skin.box);
		}

		public ElementDrawer Current
		{
			get
			{
				return new ElementDrawer (marchingRect, currentIndex + indexOffset);
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return Current;
			}
		}

		public IEnumerator<ElementDrawer> GetEnumerator ()
		{
			return this;
		}

		public bool MoveNext ()
		{
			marchingRect.y += marchingRect.height;
			currentIndex++;
			return currentIndex < indexOffset + elementCount;
		}

		public void Reset ()
		{
			throw new NotImplementedException ();
		}

		public void Dispose ()
		{
			GUI.EndGroup ();
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return this;
		}
	}
}
#endif
