using RPGCore.Items;
using System.Collections.Generic;

namespace RPGCore.Inventory.Slots
{
	public abstract class ItemSlot : IInventory
	{
		public abstract Item CurrentItem { get; }
		public abstract IEnumerable<Item> Items { get; }

		public abstract InventoryTransaction AddItem (Item item);
		public abstract InventoryTransaction SetItem (Item item);
		public abstract InventoryTransaction DestroyItem ();
		public abstract InventoryTransaction SwapInto (ItemSlot other);
		public abstract InventoryTransaction MoveInto (Inventory other);
	}
}
