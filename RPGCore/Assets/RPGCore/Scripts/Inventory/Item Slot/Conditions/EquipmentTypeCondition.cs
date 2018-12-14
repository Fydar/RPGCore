namespace RPGCore.Inventories
{
	public class EquipmentTypeCondition : ItemCondition
	{
		Slot[] slotTypes;

		public EquipmentTypeCondition (params Slot[] slots)
		{
			slotTypes = slots;
		}

		public override bool IsValid (ItemSurrogate item)
		{
			if (item == null)
				return true;

			for (int i = 0; i < slotTypes.Length; i++)
			{
				if (item.EquiptableSlot == slotTypes[i])
					return true;
			}

			return false;
		}
	}
}

