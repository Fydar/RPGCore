using RPGCore.Utility;
using System;
using UnityEngine;

namespace RPGCore.UI
{
	[Serializable] public class UIPopupButtonPool : UIPool<UIPopupButton> { }

	public class PopupMenu : MonoBehaviour
	{
		public static PopupMenu Instance;

		public CanvasGroup Background;
		public FadeBool BackgroundFade = new FadeBool ();

		[Header ("Buttons")]
		public GameObject Dialogue;
		public RectTransform ButtonsHolder;
		public UIPopupButtonPool ButtonPool;


		private void Awake ()
		{
			Close ();

			Instance = this;
		}

		private void Update ()
		{
			BackgroundFade.Update ();
			Background.alpha = BackgroundFade.Value;
			Background.blocksRaycasts = BackgroundFade.Target;

			if (Input.GetKeyDown (KeyCode.Escape))
			{
				Close ();
			}
		}

		public void Display (string header, params PopupButton[] buttons)
		{
			ButtonPool.Flush ();
			BackgroundFade.Target = true;


			for (int i = 0; i < buttons.Length; i++)
			{
				PopupButton button = buttons[i];
				UIPopupButton uiButton = ButtonPool.Grab (ButtonsHolder);
				uiButton.Setup (this, button);
			}

			Dialogue.SetActive (true);
		}

		public void Close ()
		{
			BackgroundFade.Target = false;

			Dialogue.SetActive (false);
		}
	}
}