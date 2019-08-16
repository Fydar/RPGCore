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
				if (MaxStackSize != 0 && stackableItem.Quantity > MaxStackSize)
				{
					var split = stackableItem.Take (MaxStackSize);

					StoredItem = split;
					return new InventoryResult (split, InventoryResult.OperationStatus.Partial, split.Quantity);
				}

				StoredItem = stackableItem;
				
				return new InventoryResult (stackableItem, InventoryResult.OperationStatus.Complete, stackableItem.Quantity);
			}
			else if (item is UniqueItem uniqueItem)
			{
				StoredItem = uniqueItem;

				return new InventoryResult (uniqueItem, InventoryResult.OperationStatus.Complete, 1);
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

				return new InventoryResult (null, InventoryResult.OperationStatus.Complete, stackableItem.Quantity);
			}
			else if (StoredItem is UniqueItem)
			{
				StoredItem = null;

				return new InventoryResult (null, InventoryResult.OperationStatus.Complete, 1);
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
