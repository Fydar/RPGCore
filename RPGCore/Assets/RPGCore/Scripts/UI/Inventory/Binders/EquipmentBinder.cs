using UnityEngine;

namespace RPGCore.Inventories
{
	public class EquipmentBinder : MonoBehaviour
	{
		public RPGCharacter character;
		public ItemSlotManager manager;
		public InventorySelector selector;

		[Space]
		public EquipmentEntry EquipmentSlot;
		public bool autoSetup = true;

		[Space]

		public KeyCode ActivateKey = KeyCode.None;

		private bool setup = false;

		private void Start ()
		{
			if (autoSetup)
			{
				FireSetup ();
			}
		}

		private void Update ()
		{
			if (Input.GetKeyDown (ActivateKey))
			{
				if (manager.slot.Item != null)
				{
					manager.slot.Item.TryUse ();
				}
			}
		}

		public void FireSetup ()
		{
			if (!setup)
			{
				manager.Setup (character.equipment.Items[EquipmentSlot.Index]);

				if (selector != null)
				{
					manager.onMainAction += () =>
					{
						/*selector.DisplaySelector (manager.slot, (ItemSlot returnedSlot) =>
						{
							manager.slot.Swap (returnedSlot);
						});*/
					};
				}
				setup = true;
			}
		}

		private void ActivateSlot ()
		{
			ItemSurrogate item = manager.slot.Item;

			if (item != null)
				item.TryUse ();
		}
	}
}

