using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPGCore;

namespace RPGCore.Tooltips
{
	public class EnchantmentElement : TooltipElement, ITooltipTarget<ItemSurrogate>
	{
		[SerializeField]
		private Text itemEnchantmentsText = null;

		public void Render (ItemSurrogate target)
		{
			string text = "";

			foreach (Enchantment enchantment in target.Enchantments)
			{
				if (enchantment == null)
					continue;

				text += ", ";
			}

			itemEnchantmentsText.text = text;
		}
	}
}