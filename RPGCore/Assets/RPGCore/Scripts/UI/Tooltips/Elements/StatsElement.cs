using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPGCore;

namespace RPGCore.Tooltips
{
	public class StatsElement : TooltipElement, ITooltipTarget<ItemSurrogate>
	{
		[SerializeField]
		private Text itemStats = null;

		public void Render (ItemSurrogate target)
		{
			itemStats.text = target.Description;

			/*
			string stats = "";

			StatsNode[] nodes = target.Template.GetNodes<StatsNode> ();

			for (int i = 0; i < nodes.Length; i++)
			{
				StatsNode statNode = nodes[i];

				stats += statNode.Description (target);

				if (i != nodes.Length - 1)
					stats += "\n";
			}

			itemStats.text = stats;
			*/
		}
	}
}