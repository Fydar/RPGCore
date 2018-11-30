using System;

namespace RPGCore.UI
{
	public struct PopupButton
	{
		public string Label;
		public Action OnClick;

		public PopupButton (string label, Action onClick)
		{
			Label = label;
			OnClick = onClick;
		}
	}
}

