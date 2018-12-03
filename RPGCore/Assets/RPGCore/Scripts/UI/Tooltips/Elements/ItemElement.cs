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
				statsPool.Grab (tableHolder).Setup ("Damage", weaponNode.AttackDamage[target].Value.ToString ("0") + "-" + weaponNode.AttackDamage[target].Value.ToString ("0") + " Phys");
				statsPool.Grab (tableHolder).Setup ("Attack Speed", weaponNode.AttackSpeed[target].Value.ToString ("0.00"));
				statsPool.Grab (tableHolder).Setup ("Critical Chance", weaponNode.CriticalChance[target].Value.ToString ("0.0") + " %");
				statsPool.Grab (tableHolder).Setup ("Critical Multiplier", weaponNode.CriticalMultiplier[target].Value.ToString ("0.0"));
			}

			var armourNode = target.Template.GetNode<ArmourInputNode> ();
			if (armourNode != null)
			{
				statsPool.Grab (tableHolder).Setup ("Armour", armourNode.Armour[target].Value.ToString ("0"));
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
			
			foreach (var Enchantment in target.Enchantments)
			{
				if (Enchantment == null)
					continue;

				EffectTooltipNode[] enchantmentTooltipNodes = Enchantment.Template.GetNodes<EffectTooltipNode> ();
				StatsNode[] enchantmentNodes = Enchantment.Template.GetNodes<StatsNode> ();

				if (enchantmentTooltipNodes.Length != 0 || enchantmentNodes.Length != 0)
				{
					enchantmentsHolder.gameObject.SetActive (true);
					for (int i = 0; i < enchantmentTooltipNodes.Length; i++)
					{
						EffectTooltipNode enchantmentTooltipNode = enchantmentTooltipNodes[i];

						enchantmentTextPool.Grab (enchantmentsHolder).text = enchantmentTooltipNode.Description (Enchantment);
					}
					for (int i = 0; i < enchantmentNodes.Length; i++)
					{
						StatsNode enchantmentStatNode = enchantmentNodes[i];

						Debug.Log (target);
						Debug.Log (Enchantment.Template);

						enchantmentTextPool.Grab (enchantmentsHolder).text = enchantmentStatNode.Description (Enchantment);
					}
				}
				else
				{
					enchantmentsHolder.gameObject.SetActive (false);
				}
			}
		}
	}
}
