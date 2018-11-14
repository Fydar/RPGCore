using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPGCore.UI
{
	public class UIContextFolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		public Text label;
		public RectTransform Child;

		public void Setup (UIContextMenu menu, ContextFolder entry)
		{
			label.text = entry.Label;

			for (int i = 0; i < entry.Entries.Length; i++)
			{
				entry.Entries[i].Render (menu, Child);
			}
		}

		public void OnPointerEnter (PointerEventData eventData)
		{
			Child.gameObject.SetActive (true);
		}

		public void OnPointerExit (PointerEventData eventData)
		{
			Child.gameObject.SetActive (false);
		}
	}
}