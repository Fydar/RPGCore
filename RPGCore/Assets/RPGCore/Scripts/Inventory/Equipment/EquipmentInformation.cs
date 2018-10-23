using UnityEngine;

namespace RPGCore.Inventories
{
	[CreateAssetMenu (menuName = "RPGCore/Equipment/Info")]
	public class EquipmentInformation : ScriptableObject
	{
		[Header ("General")]
		public string Name;

		public Slot AllowedItems;

		[Header ("Render")]
#if ASSET_ICONS
		[AssetIcon]
#endif
		public Sprite SlotIcon;

		public ItemSlot GenerateSlot (RPGCharacter character)
		{
			ItemSlotBehaviour[] slotBehaviour = new ItemSlotBehaviour[] {
				new Slot_EquippableSlot ()
			};

			ItemCondition[] slotCondition = new ItemCondition[] {
				new EquipmentTypeCondition (AllowedItems)
			};

			ItemStorageSlot storageSlot = new ItemStorageSlot (character, slotBehaviour, slotCondition);

			storageSlot.SlotDecoration = SlotIcon;

			return storageSlot;
		}
	}
}