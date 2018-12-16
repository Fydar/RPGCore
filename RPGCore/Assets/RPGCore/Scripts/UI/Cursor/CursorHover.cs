using RPGCore.Audio;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPGCore.UI.CursorManagement
{
	public class CursorHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
	{
		public SfxGroup HoverSound;
		public SfxGroup ClickSound;
		private Button button;

		private void Awake ()
		{
			button = GetComponent<Button> ();
		}

		public void OnPointerEnter (PointerEventData eventData)
		{
			if (button != null && !button.interactable)
				return;

			CursorManager.SetCursor ("Hand");

			if (HoverSound)
				AudioManager.Play (HoverSound);
		}

		public void OnPointerExit (PointerEventData eventData)
		{
			if (button != null && !button.interactable)
				return;

			CursorManager.SetCursor ("Default");
		}

		public void OnPointerClick (PointerEventData eventData)
		{
			if (ClickSound)
				AudioManager.Play (ClickSound);
		}
	}
}
