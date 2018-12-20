using System;
using UnityEngine;

namespace RPGCore.Tables
{
	[CreateAssetMenu (menuName = "RPGCore/Enchantment/Table")]
	public class EnchantmentTable : EnchantmentSelector
	{
		[Serializable]
		public class EnchantmentEntry : GenericTableEntry<EnchantmentSelector>
		{
			public EnchantmentEntry (EnchantmentSelector item, float balance)
				: base (item, balance) { }
		}

		[Serializable]
		public class EnchantmentRoll : AssetSelector<EnchantmentSelector, EnchantmentEntry> { }

		public string CategoryName = "Weapon";
		public EnchantmentRoll PossibleEnchantments;

		public override EnchantmentTemplate GetEnchantment ()
		{
			var enchantment = PossibleEnchantments.Select ();
			if (enchantment == null)
				return null;
			return enchantment.GetEnchantment();
		}

		public override string ToString ()
		{
			return "a random " + CategoryName + " enchantment";
		}
	}
}
