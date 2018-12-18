using RPGCore.Behaviour;
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
			Description.text = target.Description;

			var equippableNode = target.Template.GetNode<EquiptableItemNode> ();
			if (equippableNode != null)
			{
				if (equippableNode.RequiredLevel != 0)
				{
					itemRequirements.text = "Requires Level <color=#fff>" + equippableNode.RequiredLevel + "</color>";
				}
			}
			else
			{
				itemRequirements.text = "";
			}

			var weaponNode = target.Template.GetNode<WeaponInputNode> ();
			if (weaponNode != null)
			{
				statsPool.Grab (tableHolder).Setup ("Damage", weaponNode.GetStats(target).Attack.Value.ToString ("0") + " Phys", weaponNode.GetStats (target).Attack.ModifiersCount > 1);
				statsPool.Grab (tableHolder).Setup ("Attack Speed", weaponNode.GetStats (target).AttackSpeed.Value.ToString ("0.00"), weaponNode.GetStats (target).AttackSpeed.ModifiersCount > 1);
				statsPool.Grab (tableHolder).Setup ("Critical Chance", weaponNode.GetStats (target).CriticalStrikeChance.Value.ToString ("0.0") + " %", weaponNode.GetStats (target).CriticalStrikeChance.ModifiersCount > 1);
				statsPool.Grab (tableHolder).Setup ("Critical Multiplier", weaponNode.GetStats (target).CriticalStrikeMultiplier.Value.ToString ("0.0"), weaponNode.GetStats (target).CriticalStrikeMultiplier.ModifiersCount > 1);
			}

			var armourNode = target.Template.GetNode<ArmourInputNode> ();
			if (armourNode != null)
			{
				statsPool.Grab (tableHolder).Setup ("Armour", armourNode.GetStats (target).Armour.Value.ToString ("0"), armourNode.GetStats (target).Armour.ModifiersCount > 1);
			}

			INodeDescription[] tooltipNodes = target.Template.GetNodes<INodeDescription> ();
			if (tooltipNodes.Length != 0)
			{
				effectsHolder.gameObject.SetActive (true);
				for (int i = 0; i < tooltipNodes.Length; i++)
				{
					effectTextPool.Grab (effectsHolder).text = tooltipNodes[i].Description (target);
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

				INodeDescription[] enchantmentTooltipNodes = Enchantment.Template.GetNodes<INodeDescription> ();
				if (enchantmentTooltipNodes.Length != 0)
				{
					found = true;
					for (int i = 0; i < enchantmentTooltipNodes.Length; i++)
					{
						enchantmentTextPool.Grab (enchantmentsHolder).text = enchantmentTooltipNodes[i].Description (Enchantment);
					}
				}
			}
			enchantmentsHolder.gameObject.SetActive (found);
		}
	}
}
