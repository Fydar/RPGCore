using RPGCore.Items;
using System.Collections.Generic;

namespace RPGCore.Inventory.Slots
{
	public interface IInventory
	{
		IEnumerable<IItem> Items { get; }

		InventoryTransaction AddItem(IItem item);
	}
}
