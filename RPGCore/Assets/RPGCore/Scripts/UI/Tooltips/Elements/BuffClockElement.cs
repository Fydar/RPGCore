using UnityEngine;
using UnityEngine.UI;

namespace RPGCore.Tooltips
{
	public class BuffClockElement : TooltipElement, ITooltipTarget<Buff>
	{
		[SerializeField] private Image filled = null;

		private Buff buff;

		public void Render (Buff target)
		{
			buff = target;

			if (!buff.HasActiveClock)
				gameObject.SetActive (false);
		}

		private void Update ()
		{
			if (buff == null)
				return;

			filled.fillAmount = buff.DisplayPercent;
		}
	}
}
