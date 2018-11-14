namespace RPGCore.Inventories
{
	public class Slot_EquippableSlot : ItemSlotBehaviour
	{

		public override void OnEnterSlot (ItemSlot target)
		{
			EquiptableNode equiptable = target.Item.Template.GetNode<EquiptableNode> ();

			if (equiptable == null)
				return;

			equiptable.Equipped[target.Item].Value = true;
		}

		public override void OnExitSlot (ItemSlot target)
		{
			EquiptableNode equiptable = target.Item.Template.GetNode<EquiptableNode> ();

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