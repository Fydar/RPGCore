using UnityEngine;

namespace RPGCore
{
	public abstract class EnchantmentSelector : ScriptableObject
	{
		public abstract EnchantmentTemplate GetEnchantment ();
	}
}
