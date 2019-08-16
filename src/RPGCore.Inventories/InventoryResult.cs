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

		public OperationStatus Status;
		public int Quantity;

		public InventoryResult (OperationStatus status, int quantity)
		{
			Status = status;
			Quantity = quantity;
		}
	}
}
