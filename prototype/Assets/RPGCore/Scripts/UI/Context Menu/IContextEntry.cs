using UnityEngine;

namespace RPGCore.UI
{
	public interface IContextEntry
	{
		void Render (UIContextMenu menu, RectTransform holder);
	}
}

