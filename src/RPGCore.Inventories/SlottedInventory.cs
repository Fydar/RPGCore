using RPGCore.Items;
using System.Collections.Generic;
using System.Linq;

namespace RPGCore.Inventory.Slots
{
	public class SlottedInventory : IInventory
	{
		private readonly List<IItemSlot> Slots;

		public int Capacity { get; set; }
		public IItemSlotFactory ItemSlotFactory { get; }

		public SlottedInventory (int capacity, IItemSlotFactory itemSlotFactory)
		{
			Capacity = capacity;
			ItemSlotFactory = itemSlotFactory;
			Slots = new List<IItemSlot> (capacity);

			for (int i = 0; i < capacity; i++)
			{
				Slots[i] = ItemSlotFactory.Build ();
			}
		}


		public IEnumerable<IItem> Items => Slots.Select (slot => slot.CurrentItem);

		public InventoryTransaction AddItem (IItem item)
		{
			foreach (var slot in Slots)
			{
				var result = slot.AddItem (item);

				if (result.Status == TransactionStatus.Complete)
				{
					return result;
				}
			}

			return InventoryTransaction.None;
		}
	}
}
