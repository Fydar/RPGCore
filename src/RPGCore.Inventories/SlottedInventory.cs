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
		public IInventory Parent { get; }

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
