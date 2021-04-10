using RPGCore.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RPGCore.Inventory.Slots
{
	public class SlottedInventory : IInventory
	{
		private readonly List<IItemSlot> slots;

		public int Capacity { get; set; }
		public IItemSlotFactory ItemSlotFactory { get; }

		/// <inheritdoc/>
		public IInventory Parent { get; }

		/// <inheritdoc/>
		public IEnumerable<IItem> Items => slots.Select(slot => slot.CurrentItem);

		public SlottedInventory(int capacity, IItemSlotFactory itemSlotFactory, IInventory parent = null)
		{
			Capacity = capacity;
			ItemSlotFactory = itemSlotFactory ?? throw new ArgumentNullException(nameof(itemSlotFactory));
			Parent = parent;

			slots = new List<IItemSlot>(capacity);

			for (int i = 0; i < capacity; i++)
			{
				var slot = ItemSlotFactory.Build(parent);
				slots.Add(slot);
			}
		}

		/// <inheritdoc/>
		public InventoryTransaction AddItem(IItem item)
		{
			foreach (var slot in slots)
			{
				var result = slot.AddItem(item);

				if (result.Status == TransactionStatus.Complete)
				{
					return result;
				}
			}

			return InventoryTransaction.None;
		}
	}
}
