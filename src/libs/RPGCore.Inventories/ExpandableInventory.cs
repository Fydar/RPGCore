using RPGCore.Items;
using System;
using System.Collections.Generic;

namespace RPGCore.Inventory.Slots
{
	public class ExpandableInventory : IInventory
	{
		/// <inheritdoc/>
		public IInventory Parent { get; }

		public ExpandableInventory(IInventory parent)
		{
			Parent = parent ?? throw new ArgumentNullException(nameof(parent));
		}

		/// <inheritdoc/>
		public IEnumerable<IItem> Items => throw new NotImplementedException();

		/// <inheritdoc/>
		public InventoryTransaction AddItem(IItem item)
		{
			return InventoryTransaction.None;
		}
	}
}
