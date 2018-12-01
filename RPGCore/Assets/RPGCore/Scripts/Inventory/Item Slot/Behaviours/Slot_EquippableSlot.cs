namespace RPGCore.Inventories
{
	public class Slot_EquippableSlot : ItemSlotBehaviour
	{

		public override void OnEnterSlot (ItemSlot target)
		{
			EquiptableItemNode equiptable = target.Item.Template.GetNode<EquiptableItemNode> ();

			if (equiptable == null)
				return;

			equiptable.Equipped[target.Item].Value = true;
		}

		public override void OnExitSlot (ItemSlot target)
		{
			EquiptableItemNode equiptable = target.Item.Template.GetNode<EquiptableItemNode> ();

			if (equiptable == null)
				return;

			equiptable.Equipped[target.Item].Value = false;
		}

		public override void OnSlotEnable (ItemSlot target)
		{

		}

		public override void OnSlotDisable (ItemSlot target)
		{

		}
	}
}

