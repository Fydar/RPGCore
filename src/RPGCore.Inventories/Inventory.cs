using RPGCore.Items;
using System.Collections.Generic;

namespace RPGCore.Inventory.Slots
{
	public abstract class Inventory : IInventory
	{
		public abstract IEnumerable<Item> Items { get; }

		public abstract InventoryTransaction AddItem (Item item);
	}
}
