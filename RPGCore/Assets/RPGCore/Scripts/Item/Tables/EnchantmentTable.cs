using System;
using UnityEngine;

namespace RPGCore.Tables
{
	[CreateAssetMenu (menuName = "RPGCore/Enchantment Table")]
	public class EnchantmentTable : EnchantmentSelector
	{
		public string CategoryName = "Weapon";

		[Serializable]
		public class EnchantmentEntry : GenericTableEntry<EnchantmentTemplate>
		{
			public EnchantmentEntry (EnchantmentTemplate item, float balance)
				: base (item, balance) { }
		}

		[Serializable]
		public class EnchantmentRoll : AssetSelector<EnchantmentTemplate, EnchantmentEntry> { }

		public EnchantmentRoll PossibleEnchantments;

		public override EnchantmentTemplate GetEnchantment ()
		{
			return PossibleEnchantments.Select ();
		}

		public override string ToString ()
		{
			return "a random " + CategoryName + " enchantment";
		}
	}
}