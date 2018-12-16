using System.Collections.Generic;
using UnityEngine;

namespace RPGCore.UI.CursorManagement
{
	public static class CursorManager
	{
		public static List<CursorStyle> Styles;
		public static CursorStyle CurrentStyle;

		[RuntimeInitializeOnLoadMethod (RuntimeInitializeLoadType.BeforeSceneLoad)]
		static void OnRuntimeMethodLoad ()
		{
			Styles = new List<CursorStyle> (Resources.LoadAll<CursorStyle> ("Cursor Styles/"));

			SetCursor (GetStyle ("Default"));
		}

		public static void SetCursor (string style)
		{
			SetCursor (GetStyle (style));
		}

		public static void SetCursor (CursorStyle style)
		{
			Cursor.SetCursor (style.Graphic, style.Hotspot, CursorMode.Auto);
			CurrentStyle = style;
		}

		public static CursorStyle GetStyle (string name)
		{
			if (Styles == null)
				Styles = new List<CursorStyle> (Resources.LoadAll<CursorStyle> ("Cursor Styles/"));

			foreach (var style in Styles)
			{
				if (style.name == name)
					return style;
			}
			return null;
		}
	}
}
