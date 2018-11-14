using System.Collections.Generic;
using UnityEngine;

namespace RPGCore.Inventories
{
	public class EquipmentManager : InventoryRendererBase
	{
		public EquipmentSlotManagerCollection Slots;

		[Header ("State")]
		public RPGCharacter target;
		public InventorySelector selector;

		public void Start ()
		{
			Setup (target.equipment);
		}

		protected override void OnSlotAdd (ItemSlotManager slot)
		{
			/*selector.Setup (target.inventory);
			slot.onMainAction += () => {
                selector.DisplaySelector (slot.slot, (ItemSlot returnedSlot) => {
					slot.slot.Swap (returnedSlot);
				});
			};*/
		}

		public override void Setup (Inventory inventory)
		{
			if (current != null)
				return;

			current = inventory;

			managers = new List<ItemSlotManager> (current.Size);

			IEnumerator<ItemSlot> slot = current.Items.GetEnumerator ();
			IEnumerator<ItemSlotManager> manager = Slots.GetEnumerator ();

			while (slot.MoveNext ())
			{
				manager.MoveNext ();

				if (manager.Current == null)
					continue;

				manager.Current.Setup (slot.Current);
				OnSlotAdd (manager.Current);
			}
		}
	}
}