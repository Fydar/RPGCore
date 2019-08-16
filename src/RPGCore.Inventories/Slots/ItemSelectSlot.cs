using System;
using RPGCore.Items;

namespace RPGCore.Inventory.Slots
{
	public class ItemSelectSlot : ItemSlot
	{
		private Item SelectedItem;

		public override Item CurrentItem => SelectedItem;

		public override InventoryResult AddItem (Item item) => throw new NotImplementedException ();

		public override InventoryResult MoveInto (Inventory other) => throw new NotImplementedException ();

		public override InventoryResult RemoveItem () => throw new NotImplementedException ();

		public override InventoryResult SetItem (Item item) => throw new NotImplementedException ();

		public override InventoryResult SwapInto (ItemSlot other) => throw new NotImplementedException ();
	}
}
