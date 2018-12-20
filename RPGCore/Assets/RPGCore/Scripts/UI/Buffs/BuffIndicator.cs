using RPGCore.Tooltips;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPGCore
{
	[RequireComponent (typeof (RectTransform))]
	public sealed class BuffIndicator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		public Image Icon;
		public Image Cooldown;
		public Text StackSize;

		private Buff buff;
		private bool hovering;
		private RectTransform rectTransform;

		public void Setup (BuffsBar parent, Buff buff)
		{
			this.buff = buff;

			Action removeCallback = null;
			removeCallback = () =>
			{
				buff.OnRemove -= removeCallback;

				if (hovering)
					Unhover ();

				parent.Indicator.Return (this);
			};

			buff.OnRemove += removeCallback;

			Icon.sprite = buff.buffTemplate.Icon;
			Cooldown.sprite = buff.buffTemplate.Icon;

			Action stackSizeCallback = () =>
			{
				if (buff.StackSize.Value > 1)
				{
					StackSize.gameObject.SetActive (true);

					StackSize.text = buff.StackSize.Value.ToString ();
				}
				else
				{
					StackSize.gameObject.SetActive (false);
				}
			};
			buff.StackSize.onChanged += stackSizeCallback;
			stackSizeCallback ();
		}

		public void Unhover ()
		{
			TooltipManager.instance.Hide ();
			hovering = false;
		}

		public void Hover ()
		{
			TooltipManager.instance.BuffTooltip (rectTransform, buff);
			hovering = true;
		}

		private void Awake ()
		{
			rectTransform = GetComponent<RectTransform> ();
		}

		private void Update ()
		{
			if (buff == null)
				return;

			if (buff.DisplayPercent == 0.0f)
			{
				Cooldown.gameObject.SetActive (false);
				return;
			}
			Cooldown.gameObject.SetActive (true);

			Cooldown.fillAmount = 1.0f - buff.DisplayPercent;
		}

		void IPointerEnterHandler.OnPointerEnter (PointerEventData eventData)
		{
			Hover ();
		}

		void IPointerExitHandler.OnPointerExit (PointerEventData eventData)
		{
			Unhover ();
		}
	}
}

