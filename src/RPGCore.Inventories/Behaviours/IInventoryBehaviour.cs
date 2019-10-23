using RPGCore.Items;

namespace RPGCore.Inventory
{
	public interface IInventoryBehaviour
	{
		void OnItemEnter (Item item);
		void OnItemExit (Item item);
	}
}
