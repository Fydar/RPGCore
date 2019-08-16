using RPGCore.Items;

namespace RPGCore.Inventory.Slots
{
	public struct InventoryResult
	{
		public enum OperationStatus
		{
			None = 0,
			Partial = 1,
			Complete = 2,
			Referenced = 3
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
	}
}
