using System;
using UnityEngine;

namespace RPGCore.UI
{
	[Serializable]
	public class Popup
	{
		public string Header;
		[TextArea (3, 5)]
		public string Description;
		public PopupButton[] Buttons;

		public Popup (string header, string description, params PopupButton[] buttons)
		{
			Header = header;
			Description = description;
			Buttons = buttons;
		}
	}
}

