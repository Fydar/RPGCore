using RPGCore.Items;

namespace RPGCore.Inventory.Slots
{
	public class ItemTransaction
	{
		public IItem Item;
		public IInventory FromInventory;
		public IInventory ToInventory;
		public ItemTransactionType Type;
		public int Quantity;
	}
}
