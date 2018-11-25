using UnityEngine;
using UnityEngine.UI;

namespace RPGCore.Tooltips
{
	public class StatsElement : TooltipElement, ITooltipTarget<ItemSurrogate>
	{
		[SerializeField]
		private Text itemStats;

		public void Render (ItemSurrogate target)
		{
			itemStats.text = target.Description;
		}
	}
}
