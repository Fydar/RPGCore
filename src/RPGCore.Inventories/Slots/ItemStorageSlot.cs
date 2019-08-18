using System;
using System.Collections.Generic;
using System.Diagnostics;
using RPGCore.Items;

namespace RPGCore.Inventory.Slots
{
	public class ItemStorageSlot : ItemSlot
	{
		public int MaxStackSize;

		private Item StoredItem;

		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		public override Item CurrentItem => StoredItem;

		public IInventoryConstraint[] Constraints { get; }

		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		public override IEnumerable<Item> Items
		{
			get
			{
				if (StoredItem == null)
				{
					yield break;
				}

				yield return StoredItem;
			}
		}

		public ItemStorageSlot (IInventoryConstraint[] constraints = null)
		{
			Constraints = constraints;
		}

		public override InventoryResult AddItem (Item item)
		{
			if (item is null)
			{
				throw new ArgumentNullException (nameof (item), "Cannot add \"null\" item to storage slot.");
			}

			if (item is StackableItem stackableItem)
			{
				if (StoredItem == null)
				{
					if (MaxStackSize != 0 && stackableItem.Quantity > MaxStackSize)
					{
						// Split stack of items into a new stack.
						var split = stackableItem.Take (MaxStackSize);

						StoredItem = split;
						return new InventoryResult (split, InventoryResult.OperationStatus.Partial, split.Quantity);
					}
					else
					{
						// Move entire stack of items into this slot.
						StoredItem = stackableItem;

						return new InventoryResult (stackableItem, InventoryResult.OperationStatus.Complete, stackableItem.Quantity);
					}
				}
				else
				{
					if (StoredItem.Template == stackableItem.Template)
					{
						// Transfer quantities of items from one item to another.
						int maxStackSize = Math.Min(MaxStackSize, stackableItem.MaxStackSize);
						var currentStackableItem = (StackableItem)StoredItem;

						int quantityToAdd = int.MaxValue;
						if (maxStackSize != 0)
						{
							quantityToAdd = Math.Min (stackableItem.Quantity, maxStackSize - currentStackableItem.Quantity);
						}

						if (Constraints != null)
						{
							foreach (var constraint in Constraints)
							{
								quantityToAdd = Math.Min (quantityToAdd, constraint.QuantityCanAdd (this, stackableItem));
							}
						}

						if (quantityToAdd == 0)
						{
							return InventoryResult.None;
						}
						else
						{
							currentStackableItem.Quantity += quantityToAdd;
							stackableItem.Quantity -= quantityToAdd;

							return new InventoryResult (currentStackableItem, InventoryResult.OperationStatus.Complete, quantityToAdd);
						}
					}
					else
					{
						return InventoryResult.None;
					}
				}
			}
			else if (item is UniqueItem uniqueItem)
			{
				if (StoredItem != null)
				{
					return InventoryResult.None;
				}
				StoredItem = uniqueItem;

				return new InventoryResult (uniqueItem, InventoryResult.OperationStatus.Complete, 1);
			}
			else
			{
				throw new InvalidOperationException ($"Item in neither a {nameof (StackableItem)} nor a {nameof (UniqueItem)}.");
			}
		}

		public override InventoryResult MoveInto (Inventory other)
		{
			if (other is null)
			{
				throw new ArgumentNullException (nameof (other), "Cannot move into a \"null\" inventory.");
			}

			throw new NotImplementedException ();
		}

		public override InventoryResult RemoveItem ()
		{
			if (StoredItem is StackableItem stackableItem)
			{
				StoredItem = null;

				return new InventoryResult (null, InventoryResult.OperationStatus.Complete, stackableItem.Quantity);
			}
			else if (StoredItem is UniqueItem)
			{
				StoredItem = null;

				return new InventoryResult (null, InventoryResult.OperationStatus.Complete, 1);
			}
			else
			{
				throw new InvalidOperationException ($"Item in neither a {nameof (StackableItem)} nor a {nameof (UniqueItem)}.");
			}
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
				throw new ArgumentNullException (nameof (other), $"Cannot swap into a \"null\" {nameof(ItemSlot)}.");
			}

			throw new NotImplementedException ();
		}
	}
}
