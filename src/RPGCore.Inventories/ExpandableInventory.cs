using RPGCore.Items;
using System;
using System.Collections.Generic;

namespace RPGCore.Inventory.Slots
{
	public class ExpandableInventory : IInventory
	{
		public IInventory Parent { get; }

		public ExpandableInventory(IInventory parent)
		{
			Parent = parent ?? throw new ArgumentNullException (nameof (parent));
		}

		public IEnumerable<IItem> Items
		{
			get
			{
				throw new NotImplementedException ();
			}
		}

		public InventoryTransaction AddItem(IItem item)
		{


			return InventoryTransaction.None;
		}
	}
}
