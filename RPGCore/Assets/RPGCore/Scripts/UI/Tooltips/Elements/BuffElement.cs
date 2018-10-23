using UnityEngine;
using UnityEngine.UI;
//using RPGCore.Buffs;

namespace RPGCore.Tooltips
{
	public class BuffElement : TooltipElement, ITooltipTarget<Buff>
	{
		[SerializeField] private Image icon = null;
		[SerializeField] private Text description = null;

		public void Render (Buff target)
		{
			icon.sprite = target.buffTemplate.Icon;
			description.text = target.buffTemplate.Description;
		}
	}
}