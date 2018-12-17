using RPGCore.Inventories;
using RPGCore.UI;
using UnityEngine;
using UnityEngine.UI;

namespace RPGCore.Tooltips
{
	public class ItemElement : TooltipElement, ITooltipTarget<ItemSurrogate>
	{
		[Header ("General")]
		[SerializeField] private ItemRenderer slotRender;
		[SerializeField] private Text itemUsage;
		[SerializeField] private Text itemRequirements;

		[Header ("Local Stats")]
		[SerializeField] private RectTransform tableHolder;
		[SerializeField] private UIKeyValuePool statsPool;

		[Header ("Item Effects")]
		[SerializeField] private RectTransform effectsHolder;
		[SerializeField] private TextPool effectTextPool;

		[Header ("Enchantment Effects")]
		[SerializeField] private RectTransform enchantmentsHolder;
		[SerializeField] private TextPool enchantmentTextPool;

		[Header ("Description")]
		[SerializeField] private Text Description;

		public void Render (ItemSurrogate target)
		{
			statsPool.Flush ();
			effectTextPool.Flush ();
			enchantmentTextPool.Flush ();

			slotRender.RenderItem (target);
			itemUsage.text = target.EquiptableSlot.ToString ();
			itemRequirements.text = "Requires Level <color=#fff>35</color>";
			Description.text = target.Description;

			var weaponNode = target.Template.GetNode<WeaponInputNode> ();
			if (weaponNode != null)
			{
				statsPool.Grab (tableHolder).Setup ("Damage", weaponNode.GetStats(target).Attack.Value.ToString ("0") + "-" + weaponNode.GetStats (target).Attack.Value.ToString ("0") + " Phys", weaponNode.GetStats (target).Attack.ModifiersCount > 1);
				statsPool.Grab (tableHolder).Setup ("Attack Speed", weaponNode.GetStats (target).AttackSpeed.Value.ToString ("0.00"), weaponNode.GetStats (target).AttackSpeed.ModifiersCount > 1);
				statsPool.Grab (tableHolder).Setup ("Critical Chance", weaponNode.GetStats (target).CriticalStrikeChance.Value.ToString ("0.0") + " %", weaponNode.GetStats (target).CriticalStrikeChance.ModifiersCount > 1);
				statsPool.Grab (tableHolder).Setup ("Critical Multiplier", weaponNode.GetStats (target).CriticalStrikeMultiplier.Value.ToString ("0.0"), weaponNode.GetStats (target).CriticalStrikeMultiplier.ModifiersCount > 1);
			}

			var armourNode = target.Template.GetNode<ArmourInputNode> ();
			if (armourNode != null)
			{
				statsPool.Grab (tableHolder).Setup ("Armour", armourNode.GetStats (target).Armour.Value.ToString ("0"), armourNode.GetStats (target).Armour.ModifiersCount > 1);
			}

			EffectTooltipNode[] tooltipNodes = target.Template.GetNodes<EffectTooltipNode> ();
			StatsNode[] nodes = target.Template.GetNodes<StatsNode> ();

			if (tooltipNodes.Length != 0 || nodes.Length != 0)
			{
				effectsHolder.gameObject.SetActive (true);
				for (int i = 0; i < tooltipNodes.Length; i++)
				{
					EffectTooltipNode statNode = tooltipNodes[i];

					effectTextPool.Grab (effectsHolder).text = statNode.Description (target);
				}
				for (int i = 0; i < nodes.Length; i++)
				{
					StatsNode statNode = nodes[i];

					effectTextPool.Grab (effectsHolder).text = statNode.Description (target);
				}
			}
			else
			{
				effectsHolder.gameObject.SetActive (false);
			}

			bool found = false;
			foreach (var Enchantment in target.Enchantments)
			{
				if (Enchantment == null)
					continue;

				EffectTooltipNode[] enchantmentTooltipNodes = Enchantment.Template.GetNodes<EffectTooltipNode> ();
				StatsNode[] enchantmentNodes = Enchantment.Template.GetNodes<StatsNode> ();
				GrantWeaponStatsNode[] weaponStats = Enchantment.Template.GetNodes<GrantWeaponStatsNode> ();
				ArmourGrantNode[] armourStats = Enchantment.Template.GetNodes<ArmourGrantNode> ();

				if (enchantmentTooltipNodes.Length != 0 || enchantmentNodes.Length != 0 ||
					weaponStats.Length != 0 || armourStats.Length != 0)
				{
					found = true;
					for (int i = 0; i < enchantmentTooltipNodes.Length; i++)
					{
						enchantmentTextPool.Grab (enchantmentsHolder).text = enchantmentTooltipNodes[i].Description (Enchantment);
					}
					for (int i = 0; i < enchantmentNodes.Length; i++)
					{
						enchantmentTextPool.Grab (enchantmentsHolder).text = enchantmentNodes[i].Description (Enchantment);
					}
					for (int i = 0; i < weaponStats.Length; i++)
					{
						enchantmentTextPool.Grab (enchantmentsHolder).text = weaponStats[i].Description (Enchantment);
					}
					for (int i = 0; i < armourStats.Length; i++)
					{
						enchantmentTextPool.Grab (enchantmentsHolder).text = armourStats[i].Description (Enchantment);
					}
				}
			}
			enchantmentsHolder.gameObject.SetActive (found);
		}
	}
}
