using System;

namespace RPGCore.Items
{
	/// <summary>
	/// Stackable items have a quantity and all behaviours can't store data on a single item.
	/// </summary>
	public class StackableItem : Item
	{
		public int Quantity { get; set; }
	}
}
