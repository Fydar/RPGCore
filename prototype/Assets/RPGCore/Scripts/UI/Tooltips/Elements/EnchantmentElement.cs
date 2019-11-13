﻿using UnityEngine;
using UnityEngine.UI;

namespace RPGCore.Tooltips
{
	public class EnchantmentElement : TooltipElement, ITooltipTarget<ItemSurrogate>
	{
		[SerializeField]
		private Text itemEnchantmentsText;

		public void Render(ItemSurrogate target)
		{
			string text = "";

			foreach (var enchantment in target.Enchantments)
			{
				if (enchantment == null)
				{
					continue;
				}

				text += ", ";
			}

			itemEnchantmentsText.text = text;
		}
	}
}
