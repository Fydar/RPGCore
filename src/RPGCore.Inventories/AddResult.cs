namespace RPGCore.Inventory.Slots
{
	public struct AddResult
	{
		public enum AddStatus
		{
			None = 0,
			Partial = 1,
			Complete = 2,
			Referenced = 3
		}

		public AddStatus Status;
		public int Quantity;

		public AddResult (AddStatus status, int quantity)
		{
			Status = status;
			Quantity = quantity;
		}
	}
}
