using RPGCore.Utility;
using RPGCore.Utility.InspectorLog;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPGCore.UI
{
	public class Chat : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		public enum State
		{
			Hidden,
			FadingIn,
			Waiting,
			FadingOut
		}

		public static Chat Instance;

		[SerializeField] private InspectorLog editorLog;
		[SerializeField] private InputField inputField;

		[Header ("Text")]
		[SerializeField] private RectTransform holder;
		[SerializeField] private TextPool textElements;

		[Header ("Fading")]
		[SerializeField] private CanvasGroup chatFader;
		[SerializeField] private DuelFadeBool hoverFade;
		[SerializeField] private DuelFadeBool inputtingFade;

		[SerializeField] private float fadeOutAlpha = 0.2f;
		[SerializeField] private float fadeInAlpha = 1.0f;
		[Space]
		[SerializeField] private float messageFadeIn = 0.3f;
		[SerializeField] private float messageWait = 3.7f;
		[SerializeField] private float messageFadeOut = 2f;

		private State state = State.Hidden;
		private float currentTime;

		private void Awake ()
		{
			Instance = this;

			textElements.Flush ();
		}

		private void Update ()
		{
			inputtingFade.Target = inputField.isFocused;

			if (chatFader != null)
			{
				switch (state)
				{
					case State.FadingIn:
						currentTime += Time.deltaTime / messageFadeIn;
						chatFader.alpha = Mathf.Lerp (fadeOutAlpha, fadeInAlpha,
							Mathf.Max (hoverFade.Update (), currentTime, inputtingFade.Update ()));

						if (currentTime >= 1.0f)
						{
							state = State.Waiting;
							currentTime = 0.0f;
						}
						break;
					case State.Waiting:
						currentTime += Time.deltaTime / messageWait;

						if (currentTime >= 1.0f)
						{
							state = State.FadingOut;
							currentTime = 0.0f;
						}
						break;
					case State.FadingOut:
						currentTime += Time.deltaTime / messageFadeOut;
						chatFader.alpha = Mathf.Lerp (fadeOutAlpha, fadeInAlpha,
							Mathf.Max (hoverFade.Update (), 1.0f - currentTime, inputtingFade.Update ()));

						if (currentTime >= 1.0f)
						{
							state = State.Hidden;
							currentTime = 0.0f;
						}
						break;
					default:
						chatFader.alpha = Mathf.Max (Mathf.Lerp (fadeOutAlpha, fadeInAlpha, hoverFade.Update ()), inputtingFade.Update ());
						break;
				}
			}
		}

		public void Log (string text)
		{
			Text textElement = textElements.Grab (holder);

			textElement.text = text;
			editorLog.Log (text);

			if (state == State.FadingOut)
			{
				state = State.FadingIn;
				currentTime = 1.0f - currentTime;
			}
			else if (state == State.Waiting)
			{
				currentTime = 0.0f;
			}
			else if (state == State.Hidden)
			{
				state = State.FadingIn;
				currentTime = 0.0f;
			}
		}

		void IPointerEnterHandler.OnPointerEnter (PointerEventData eventData)
		{
			hoverFade.Target = true;
		}

		void IPointerExitHandler.OnPointerExit (PointerEventData eventData)
		{
			hoverFade.Target = false;
		}
	}
}
