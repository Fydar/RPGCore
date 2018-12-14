using UnityEngine;

namespace RPGCore.Inventories
{
	public class InventoryRenderer : ExpandableInventoryRenderer
	{
		[Header ("State")]
		public RPGCharacter target;

		private void Start ()
		{
			Setup (target.inventory);
		}

		protected override void OnSlotAdd (ItemSlotManager manager)
		{
			//TODO: Open options pancel

			/*manager.onMainAction += () => {
				selector.DisplaySelector (manager.slot, (ItemSlot returnedSlot) => {
					manager.slot.Swap (returnedSlot);
				});
			};*/
		}

		protected override void OnSlotRemove (ItemSlotManager slot)
		{

		}
	}
}

