﻿using System.Collections.Generic;
using UnityEngine;

namespace RPGCore.Inventories
{
	public abstract class FixedInventoryRenderer : InventoryRendererBase
	{
		[Header("Slots")]
		public ItemSlotManager[] Slots;

		public override void Setup(Inventory inventory)
		{
			if (current != null)
			{
				return;
			}

			current = inventory;

			managers = new List<ItemSlotManager>(current.Size);

			for (int i = 0; i < current.Items.Count; i++)
			{
				var slot = current.Items[i];
				var manager = Slots[i];

				manager.Setup(slot);
				OnSlotAdd(manager);
			}
		}
	}
}

