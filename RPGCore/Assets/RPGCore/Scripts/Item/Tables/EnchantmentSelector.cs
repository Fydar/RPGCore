using UnityEngine;
using System.Collections;

namespace RPGCore.Tables
{
	public abstract class EnchantmentSelector : ScriptableObject
	{
		public abstract EnchantmentTemplate GetEnchantment ();
	}
}