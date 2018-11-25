//#define DeveloperTooltip

using RPGCore.Inventories;
using UnityEngine;
using UnityEngine.UI;

namespace RPGCore.Tooltips
{
	public class ItemElement : TooltipElement, ITooltipTarget<ItemSurrogate>
	{
		[SerializeField] private Text itemDescription;
		[SerializeField] private ItemRenderer slotRender;
		[SerializeField] private Text itemStats;

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

			EffectTooltipNode[] tooltipNodes = target.Template.GetNodes<EffectTooltipNode> ();
			for (int i = 0; i < tooltipNodes.Length; i++)
			{
				EffectTooltipNode statNode = tooltipNodes[i];

				stats += statNode.Description (target);

				if (i != tooltipNodes.Length - 1)
					stats += "\n";
			}

			StatsNode[] nodes = target.Template.GetNodes<StatsNode> ();
			for (int i = 0; i < nodes.Length; i++)
			{
				if (i == 0 && stats != "")
					stats += "\n";

				StatsNode statNode = nodes[i];

				stats += statNode.Description (target);

				if (i != nodes.Length - 1)
					stats += "\n";
			}

			itemStats.text = stats;
		}
	}
}
