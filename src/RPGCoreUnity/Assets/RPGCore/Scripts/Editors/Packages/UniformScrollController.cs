using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPGCore.Unity.Editors
{
	public struct UniformScrollController : IEnumerable<ElementDrawer>, IEnumerator<ElementDrawer>
	{
		private static readonly float ScrollSpeed = 8;
		private static readonly float LockOffset = -10000f;

		private readonly int indexOffset;
		private readonly int elementCount;

		private Rect marchingRect;
		private int currentIndex;

		public UniformScrollController(Rect area, float elementHeight, ref Vector2 offset, int elements)
		{
			float maxOverflow = area.height % elementHeight;
			int maxScrollMinIndex = Mathf.CeilToInt((area.height + maxOverflow) / elementHeight) - 1;

			float scrollHeight = Mathf.Max(0.0f, ((elements - maxScrollMinIndex) * elementHeight) + (1 - maxOverflow));

			float viewScroll = offset.y;
			if (offset.y < -1)
			{
				viewScroll = scrollHeight;
			}

			float localOffset = viewScroll % elementHeight;
			indexOffset = Mathf.Max(0, (int)Mathf.Floor(viewScroll / elementHeight));
			elementCount = Mathf.CeilToInt((area.height + localOffset) / elementHeight);

			var verticalScrollRect = new Rect(area)
			{
				xMin = area.xMax - EditorGUIUtility.singleLineHeight
			};
			area.xMax = verticalScrollRect.xMin;

			EditorGUI.BeginChangeCheck();
			if (scrollHeight <= 0.0f)
			{
				EditorGUI.BeginDisabledGroup(true);
				GUI.VerticalScrollbar(verticalScrollRect, 0.0f, 1.0f, 0.0f, 1.0f);
				EditorGUI.EndDisabledGroup();
			}
			else
			{
				if (Event.current.rawType == EventType.ScrollWheel && area.Contains(Event.current.mousePosition))
				{
					if (Event.current.delta.y > 0)
					{
						offset.y += Event.current.delta.y * ScrollSpeed;
						if (offset.y >= scrollHeight || offset.y < 0.075f)
						{
							offset.y = LockOffset;
						}
					}
					else
					{
						if (offset.y <= LockOffset)
						{
							offset.y = scrollHeight;
						}

						offset.y += Event.current.delta.y * ScrollSpeed;
						if (offset.y < 0 && offset.y > LockOffset)
						{
							offset.y = 0;
						}
					}
					Event.current.Use();
				}

				float inputScroll = GUI.VerticalScrollbar(verticalScrollRect,
					viewScroll, area.height, 0.0f, scrollHeight + area.height);
				if (EditorGUI.EndChangeCheck())
				{
					bool hasReachedMax = inputScroll >= scrollHeight;

					offset.y = hasReachedMax ? LockOffset : inputScroll;
				}
			}

			marchingRect = new Rect(0, -localOffset - elementHeight, area.width, elementHeight);
			currentIndex = -1;

			GUI.BeginGroup(area);
		}

		public ElementDrawer Current
		{
			get
			{
				return new ElementDrawer(marchingRect, currentIndex + indexOffset);
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return Current;
			}
		}

		public IEnumerator<ElementDrawer> GetEnumerator()
		{
			return this;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this;
		}

		public bool MoveNext()
		{
			marchingRect.y += marchingRect.height;
			currentIndex++;
			return currentIndex < indexOffset + elementCount;
		}

		public void Reset()
		{
		}

		public void Dispose()
		{
			GUI.EndGroup();
		}
	}
}
