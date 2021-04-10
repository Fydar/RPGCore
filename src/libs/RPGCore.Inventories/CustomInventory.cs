﻿using RPGCore.Items;
using System;
using System.Collections.Generic;

namespace RPGCore.Inventory.Slots
{
	public class CustomInventory : IInventory
	{
		/// <inheritdoc/>
		public IInventory Parent { get; }

		public CustomInventory(IInventory parent)
		{
			Parent = parent ?? throw new ArgumentNullException(nameof(parent));
		}

		/// <inheritdoc/>
		public IEnumerable<IItem> Items
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		/// <inheritdoc/>
		public InventoryTransaction AddItem(IItem item)
		{
			return InventoryTransaction.None;
		}
	}
}
