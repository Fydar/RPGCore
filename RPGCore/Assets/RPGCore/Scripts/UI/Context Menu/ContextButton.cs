using System;
using UnityEngine;

namespace RPGCore.UI
{
	public struct ContextButton : IContextEntry
	{
		public string Label;
		public Action OnClick;

		public ContextButton (string label, Action onClick)
		{
			Label = label;
			OnClick = onClick;
		}

		public void Render (UIContextMenu menu, RectTransform holder)
		{
			UIContextButton button = menu.ButtonPool.Grab (holder);
			button.Setup (menu, this);
		}
	}

	public struct ContextNone : IContextEntry
	{
		public void Render (UIContextMenu menu, RectTransform holder)
		{

		}
	}
}