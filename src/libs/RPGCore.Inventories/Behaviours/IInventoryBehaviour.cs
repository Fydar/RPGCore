using RPGCore.Items;

namespace RPGCore.Inventory
{
	public interface IInventoryBehaviour
	{
		void OnItemEnter(IItem item);

		void OnItemExit(IItem item);
	}
}
