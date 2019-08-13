using UnityEngine;

namespace RPGCore.UI
{
	public struct ContextFolder : IContextEntry
	{
		public string Label;
		public IContextEntry[] Entries;

		public UIContextMenu Menu;

		public ContextFolder (string label, params IContextEntry[] entries)
		{
			Label = label;
			Entries = entries;
			Menu = null;
		}

		public void Render (UIContextMenu menu, RectTransform holder)
		{
			UIContextFolder button = menu.FolderPool.Grab (holder);
			button.Setup (menu, this);
			Menu = menu;
		}
	}
}

