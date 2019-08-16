using System;
using RPGCore.Items;

namespace RPGCore.Inventory.Slots
{
	public class ItemStorageSlot : ItemSlot
	{
		public int MaxStackSize;

		private Item StoredItem;

		public override Item CurrentItem => StoredItem;

		public override InventoryResult AddItem (Item item)
		{
			if (item is StackableItem stackableItem)
			{
				StoredItem = stackableItem;

				return new InventoryResult (InventoryResult.OperationStatus.Complete, stackableItem.Quantity);
			}
			else if (item is UniqueItem uniqueItem)
			{
				StoredItem = uniqueItem;

				return new InventoryResult (InventoryResult.OperationStatus.Complete, 1);
			}
			else
			{
				throw new InvalidOperationException ($"Item in neither a {nameof (StackableItem)} nor a {nameof (UniqueItem)}.");
			}
		}

		public override InventoryResult MoveInto (Inventory other) => throw new NotImplementedException ();

		public override InventoryResult RemoveItem ()
		{
			if (StoredItem is StackableItem stackableItem)
			{
				StoredItem = null;

				return new InventoryResult (InventoryResult.OperationStatus.Complete, stackableItem.Quantity);
			}
			else if (StoredItem is UniqueItem)
			{
				StoredItem = null;

				return new InventoryResult (InventoryResult.OperationStatus.Complete, 1);
			}
			else
			{
				throw new InvalidOperationException ($"Item in neither a {nameof (StackableItem)} nor a {nameof (UniqueItem)}.");
			}
		}

		public override InventoryResult SetItem (Item item) => throw new NotImplementedException ();

		public override InventoryResult SwapInto (ItemSlot other) => throw new NotImplementedException ();
	}
}
