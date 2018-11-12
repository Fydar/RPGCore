using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RPGCore.Tooltips;

namespace RPGCore.Inventories
{
	public abstract class FixedInventoryRenderer : InventoryRendererBase
	{
		[Header ("Slots")]
		public ItemSlotManager[] Slots;

		public override void Setup (Inventory inventory)
		{
			if (current != null)
				return;

			current = inventory;

			managers = new List<ItemSlotManager> (current.Size);

			for (int i = 0; i < current.Items.Count; i++)
			{
				ItemSlot slot = current.Items[i];
				ItemSlotManager manager = Slots[i];

				manager.Setup (slot);
				OnSlotAdd (manager);
			}
		}
	}
}