using RPGCore.Items;
using System;
using System.Collections.Generic;

namespace RPGCore.Inventory.Slots
{
	public abstract class Inventory : IInventory
	{
		public abstract IEnumerable<Item> Items { get; }

		public abstract InventoryResult AddItem (Item item);
	}
}
