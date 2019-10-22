﻿using RPGCore.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RPGCore.Inventory.Slots
{
	public class SlottedInventory : Inventory
	{
		private readonly List<ItemSlot> Slots;

		public int Capacity { get; set; }
		public IItemSlotFactory ItemSlotFactory { get; }

		public SlottedInventory (int capacity, IItemSlotFactory itemSlotFactory)
		{
			Capacity = capacity;
			ItemSlotFactory = itemSlotFactory;
			Slots = new List<ItemSlot> (capacity);

			for (int i = 0; i < capacity; i++)
			{
				Slots[i] = ItemSlotFactory.Build();
			}
		}


		public override IEnumerable<Item> Items
		{
			get
			{
				return Slots.Select (slot => slot.CurrentItem);
			}
		}

		public override InventoryResult AddItem (Item item)
		{


			return InventoryResult.None;
		}
	}
}