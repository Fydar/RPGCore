//#define DeveloperTooltip

using RPGCore.Inventories;
using UnityEngine;
using UnityEngine.UI;

namespace RPGCore.Tooltips
{
	public class DescriptionElement : TooltipElement, ITooltipTarget<ItemSurrogate>
	{
		[SerializeField]
		private Text itemDescription = null;

		public ItemRenderer slotRender = null;

		[SerializeField]
		private Text itemStats = null;

		public void Render (ItemSurrogate target)
		{
			string description = "";

#if DeveloperTooltip
		description += "\n========================\nDeveloper Information\n";

		description += "Seed: " + item.Seed.Value.ToString () + "\n";

		description += "========================";
#endif

			description += target.EquiptableSlot.ToString ();

			itemDescription.text = description;

			slotRender.RenderItem (target);


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
		}
	}
}