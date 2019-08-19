using RPGCore.Items;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Inventory.Slots
{
	public class ItemStorageSlot : ItemSlot
	{
		internal Item StoredItem;

		public int MaxStackSize { get; set; }

		public IInventoryConstraint[] Constraints { get; }
		public ISlotBehaviour[] Behaviours { get; }

		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		public override Item CurrentItem => StoredItem;

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

		public ItemStorageSlot (IInventoryConstraint[] constraints = null, ISlotBehaviour[] behaviours = null)
		{
			Constraints = constraints;
			Behaviours = behaviours;
		}

		public override InventoryResult AddItem (Item item)
		{
			if (item is null)
			{
				throw new ArgumentNullException (nameof (item), "Cannot add \"null\" item to storage slot.");
			}

			if (item is StackableItem stackableItem)
			{
				int itemMaxStackSize = Math.Min (MaxStackSize, stackableItem.MaxStackSize);
				if (itemMaxStackSize == 0)
				{
					itemMaxStackSize = int.MaxValue;
				}

				// Apply inventory constraints
				int constrainedMaxSize = itemMaxStackSize;
				if (Constraints != null)
				{
					foreach (var constraint in Constraints)
					{
						constrainedMaxSize = Math.Min (constrainedMaxSize, constraint.QuantityCanAdd (this, stackableItem));
					}
				}
				if (constrainedMaxSize == 0)
				{
					return InventoryResult.None;
				}

				if (StoredItem == null)
				{
					if (constrainedMaxSize != 0 && stackableItem.Quantity > constrainedMaxSize)
					{
						// Split stack of items into a new stack.
						var split = stackableItem.Take (constrainedMaxSize);

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
				else if (StoredItem.Template == stackableItem.Template)
				{
					// Transfer quantities of items from one item to another.
					var currentStackableItem = (StackableItem)StoredItem;

					// Adjust for pre-exisiting capacity.
					int quantityToAdd = constrainedMaxSize;
					if (constrainedMaxSize != 0)
					{
						quantityToAdd = Math.Min(constrainedMaxSize, 
							Math.Min (stackableItem.Quantity, itemMaxStackSize - currentStackableItem.Quantity)
						);
					}

					if (quantityToAdd > 0)
					{
						currentStackableItem.Quantity += quantityToAdd;
						stackableItem.Quantity -= quantityToAdd;

						if (stackableItem.Quantity == 0)
						{
							return new InventoryResult (currentStackableItem, InventoryResult.OperationStatus.Complete, quantityToAdd);
						}
						else
						{
							return new InventoryResult (currentStackableItem, InventoryResult.OperationStatus.Partial, quantityToAdd);
						}
					}
					else
					{
						return InventoryResult.None;
					}
				}
				else
				{
					return InventoryResult.None;
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

			StoredItem = item;

			return InventoryResult.CompleteWholeItem (StoredItem);
		}

		public override InventoryResult SwapInto (ItemSlot other)
		{
			if (other is null)
			{
				throw new ArgumentNullException (nameof (other), $"Cannot swap into a \"null\" {nameof (ItemSlot)}.");
			}

			if (other is ItemStorageSlot itemStorageSlot)
			{
				var temp = StoredItem;
				StoredItem = itemStorageSlot.StoredItem;
				itemStorageSlot.StoredItem = temp;

				return new InventoryResult (itemStorageSlot.StoredItem, InventoryResult.OperationStatus.Complete, 0);
			}
			else if (other is ItemSelectSlot itemSelectSlot)
			{
				other.SetItem (StoredItem);

				return new InventoryResult (itemSelectSlot.SelectedItem, InventoryResult.OperationStatus.Complete, 0);
			}
			else
			{
				throw new InvalidOperationException ($"Slot in neither a {nameof (ItemStorageSlot)} nor a {nameof (ItemSelectSlot)}.");
			}
		}
	}
}
