using RPGCore.Items;
using System;
using System.Collections.Generic;

namespace RPGCore.Inventory.Slots
{
	public class ExpandableInventory : Inventory
	{
		public override IEnumerable<Item> Items
		{
			get
			{
				throw new NotImplementedException ();
			}
		}

		public override InventoryResult AddItem (Item item)
		{


			return InventoryResult.None;
		}
	}
}
