using RPGCore.Items;
using System;

namespace RPGCore.Inventory.Slots
{
	public struct InventoryResult
	{
		public static readonly InventoryResult None = new InventoryResult ();

		public enum OperationStatus
		{
			None = 0,
			Partial = 1,
			Complete = 2
		}

		public Item ItemAdded;
		public OperationStatus Status;
		public int Quantity;

		public InventoryResult (Item itemAdded, OperationStatus status, int quantity)
		{
			ItemAdded = itemAdded;
			Status = status;
			Quantity = quantity;
		}

		public static InventoryResult CompleteWholeItem (Item item)
		{
			if (item is StackableItem stackableItem)
			{
				return new InventoryResult (item, OperationStatus.Complete, stackableItem.Quantity);
			}
			else if (item is UniqueItem)
			{
				return new InventoryResult (item, OperationStatus.Complete, 1);
			}
			else
			{
				throw new InvalidOperationException ($"Item in neither a {nameof (StackableItem)} nor a {nameof (UniqueItem)}.");
			}
		}
	}
}
