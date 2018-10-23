using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPGCore;
using RPGCore.Inventories;

namespace RPGCore.Tooltips
{
	public class LoreElement : TooltipElement, ITooltipTarget<ItemSurrogate>
	{
		[SerializeField]
		private Text itemDescription = null;

		public void Render (ItemSurrogate target)
		{
			string description = "";

			description += target.Description.ToString ();

			itemDescription.text = description;
		}
	}
}