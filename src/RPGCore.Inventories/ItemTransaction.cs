using RPGCore.Items;

namespace RPGCore.Inventory.Slots
{
	public class ItemTransaction
	{
		public IItem Item;
		public IInventory FromInventory;
		public IInventory ToInventory;
		public int Quantity;

		public ItemTransactionType Type
		{
			get
			{
				return CalculateType (from, to);
			}
		}

		private static ItemTransactionType CalculateType(IInventory from, IInventory to)
		{
			if (from == null)
			{
				if (to == null)
				{
					return ItemTransactionType.None;
				}
				else
				{
					return ItemTransactionType.Add;
				}
			}
			else
			{
				if (to == null)
				{
					return ItemTransactionType.Destroy;
				}
				else
				{
					return ItemTransactionType.Move;
				}
			}
		}
	}
}
