using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPGCore.Stats
{
	public class StatBar : MonoBehaviour
	{
		[Header ("Character")]
		[SerializeField]
		private RPGCharacter localCharacter = null;

		[Space]

		[SerializeField]
		[CollectionType (typeof (StateCollection<>))]
		private CollectionEntry state;

		[SerializeField]
		[CollectionType (typeof (StatCollection<>))]
		private CollectionEntry stat;

		[Header ("Text")]
		[SerializeField]
		private Text TextField = null;
		[SerializeField] private string TextFormat = "{0:#}/{1:#}";

		[Header ("Primary Bar")]
		[SerializeField]
		private Image primaryBar = null;

		[Header ("Secondary Bar")]
		[SerializeField]
		private Image secondaryBar = null;
		[SerializeField] private float secondaryBarDelay = 1.0f;

		private void Start ()
		{
			SetupReference ();
		}

		private void Update ()
		{
			if (secondaryBar != null)
			{
				if (secondaryBar.fillAmount > primaryBar.fillAmount)
					secondaryBar.fillAmount = Mathf.Lerp (secondaryBar.fillAmount, primaryBar.fillAmount, Time.unscaledDeltaTime * secondaryBarDelay);
				else
					secondaryBar.fillAmount = primaryBar.fillAmount;
			}
		}

		private void SetupReference ()
		{
			if (localCharacter == null)
				return;

			localCharacter.Stats[stat].OnValueChanged += UpdateBars;
			localCharacter.States[state].OnValueChanged += UpdateBars;

			UpdateBars ();
		}

		private void UpdateBars ()
		{
			float value = localCharacter.States[state].Value;
			float max = localCharacter.Stats[stat].Value;

			primaryBar.fillAmount = value / max;

			if (TextField != null)
			{
				TextField.text = String.Format (TextFormat, value, max);
			}
		}
	}
}