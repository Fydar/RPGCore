using RPGCore.Items;
using System.Collections.Generic;

namespace RPGCore.Inventory.Slots
{
	public interface IInventory
	{
		IEnumerable<Item> Items { get; }
	}
}
