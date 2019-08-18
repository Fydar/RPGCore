using System;
using System.Collections.Generic;
using RPGCore.Items;

namespace RPGCore.Inventory.Slots
{
	public class ItemSelectSlot : ItemSlot
	{
		private Item SelectedItem;

		public override Item CurrentItem => SelectedItem;

		public override IEnumerable<Item> Items
		{
			get
			{
				if (SelectedItem == null)
				{
					yield break;
				}

				yield return SelectedItem;
			}
		}

		public override InventoryResult AddItem (Item item)
		{
			if (item is null)
			{
				throw new ArgumentNullException (nameof (item), "Cannot add \"null\" item to storage slot.");
			}

			throw new NotImplementedException ();
		}

		public override InventoryResult MoveInto (Inventory other)
		{
			if (other is null)
			{
				throw new ArgumentNullException (nameof (other), "Cannot move into a \"null\" inventory.");
			}

			SelectedItem = null;

			return new InventoryResult (null, InventoryResult.OperationStatus.Complete, 0);
		}

		public override InventoryResult RemoveItem ()
		{
			throw new NotImplementedException ();
		}

		public override InventoryResult SetItem (Item item)
		{
			if (item is null)
			{
				throw new ArgumentNullException (nameof (item), "Cannot add \"null\" item to storage slot.");
			}

			throw new NotImplementedException ();
		}

		public override InventoryResult SwapInto (ItemSlot other)
		{
			if (other is null)
			{
				throw new ArgumentNullException (nameof (other), $"Cannot swap into a \"null\" {nameof (ItemSlot)}.");
			}

			throw new NotImplementedException ();
		}
	}
}
