using RPGCore.Items;
using System;
using System.Collections.Generic;

namespace RPGCore.Inventory.Slots
{
	public class ExpandableInventory : IInventory
	{
		public IEnumerable<IItem> Items
		{
			get
			{
				throw new NotImplementedException ();
			}
		}

		public InventoryTransaction AddItem (IItem item)
		{


			return InventoryTransaction.None;
		}
	}
}
