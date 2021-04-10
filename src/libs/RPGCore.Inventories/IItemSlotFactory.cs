namespace RPGCore.Inventory.Slots
{
	public interface IItemSlotFactory
	{
		IItemSlot Build(IInventory parent);
	}
}
