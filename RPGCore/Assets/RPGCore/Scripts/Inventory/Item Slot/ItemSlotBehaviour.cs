namespace RPGCore.Inventories
{
	public abstract class ItemSlotBehaviour
	{
		public virtual void Setup (ItemSlot target) { }
		public abstract void OnEnterSlot (ItemSlot target);
		public abstract void OnExitSlot (ItemSlot target);
		public abstract void OnSlotEnable (ItemSlot target);
		public abstract void OnSlotDisable (ItemSlot target);
	}
}