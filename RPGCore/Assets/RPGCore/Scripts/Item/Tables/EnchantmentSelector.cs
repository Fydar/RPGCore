using UnityEngine;

namespace RPGCore.Tables
{
	public abstract class EnchantmentSelector : ScriptableObject
	{
		public abstract EnchantmentTemplate GetEnchantment ();
	}
}

