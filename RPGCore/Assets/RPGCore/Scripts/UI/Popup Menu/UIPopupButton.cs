using UnityEngine;
using UnityEngine.UI;

namespace RPGCore.UI
{
	public class UIPopupButton : MonoBehaviour
	{
		public Text label;

		private PopupButton entry;
		private PopupMenu menu;

		public void Setup (PopupMenu menu, PopupButton entry)
		{
			this.menu = menu;
			this.entry = entry;

			label.text = entry.Label;
		}

		public void FireClick ()
		{
			menu.Close ();

			if (entry.OnClick != null)
				entry.OnClick ();
		}
	}
}

