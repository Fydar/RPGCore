using RPGCore.Items;

namespace RPGCore.Inventory.Slots
{
	public abstract class ItemSlot
	{
		public abstract Item CurrentItem { get; }

		public abstract InventoryResult AddItem (Item item);
		public abstract InventoryResult SetItem (Item item);
		public abstract InventoryResult RemoveItem ();
		public abstract InventoryResult SwapInto (ItemSlot other);
		public abstract InventoryResult MoveInto (Inventory other);
	}
}
