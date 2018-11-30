using UnityEngine;

namespace RPGCore.UI
{
	public struct ContextDivider : IContextEntry
	{
		public UIContextMenu Menu;

		public void Render (UIContextMenu menu, RectTransform holder)
		{
			menu.DividerPool.Grab (holder);
			Menu = menu;
		}
	}
}

