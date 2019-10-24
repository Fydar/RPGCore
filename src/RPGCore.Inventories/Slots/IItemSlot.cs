using RPGCore.Items;

namespace RPGCore.Inventory.Slots
{
	public interface IItemSlot : IInventory
	{
		IItem CurrentItem { get; }

		InventoryTransaction DestroyItem ();
		InventoryTransaction DragInto (IInventory other);
		InventoryTransaction MoveInto (IInventory other);
		InventoryTransaction SetItem (IItem item);
		InventoryTransaction Swap (IItemSlot other);
	}
}
