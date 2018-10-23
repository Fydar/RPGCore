using UnityEngine;
using UnityEngine.UI;

namespace RPGCore.UI
{
	public class UIContextButton : MonoBehaviour
	{
		public Text label;

		private ContextButton entry;
		private UIContextMenu menu;

		public void Setup (UIContextMenu menu, ContextButton entry)
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