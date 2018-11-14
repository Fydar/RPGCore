namespace RPGCore.Inventories
{
	public class Slot_Pair : ItemSlotBehaviour
	{
		public ItemSlot pair;

		public Slot_Pair (ItemSlot _pair)
		{
			pair = _pair;
		}

		public override void Setup (ItemSlot target)
		{
			pair.onAfterChanged += () =>
			{
				if (pair.Item == null)
				{
					target.Enabled = true;
					return;
				}

				if (pair.Item.EquiptableSlot == Slot.TwoHanded)
				{
					target.Return ();
					target.Enabled = false;
				}
				else
				{
					target.Enabled = true;
				}
			};
		}

		public override void OnEnterSlot (ItemSlot target)
		{
			if (pair.Item == null)
				return;

			if (pair.Item.EquiptableSlot == Slot.TwoHanded)
			{
				pair.Return ();
			}
		}

		public override void OnExitSlot (ItemSlot target)
		{

		}

		public override void OnSlotEnable (ItemSlot target)
		{

		}

		public override void OnSlotDisable (ItemSlot target)
		{

		}
	}
}