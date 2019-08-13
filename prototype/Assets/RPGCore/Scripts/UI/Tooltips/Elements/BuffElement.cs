using UnityEngine;
using UnityEngine.UI;
//using RPGCore.Buffs;

namespace RPGCore.Tooltips
{
	public class BuffElement : TooltipElement, ITooltipTarget<Buff>
	{
		[SerializeField] private Image icon;
		[SerializeField] private Text description;

		public void Render (Buff target)
		{
			icon.sprite = target.buffTemplate.Icon;
			description.text = target.buffTemplate.Description;
		}
	}
}
