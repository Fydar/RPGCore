﻿using RPGCore.Items;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Inventory.Slots
{
	public class ItemStorageSlot : IItemSlot
	{
		public IInventoryBehaviour[] Behaviours { get; }
		public IInventoryConstraint[] Constraints { get; }
		public IItem CurrentItem => storedItem;

		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		public IEnumerable<IItem> Items
		{
			get
			{
				if (storedItem == null)
				{
					yield break;
				}

				yield return storedItem;
			}
		}

		public int MaxStackSize { get; set; }

		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		internal IItem storedItem;

		public ItemStorageSlot(IInventoryConstraint[] constraints = null, IInventoryBehaviour[] behaviours = null)
		{
			Constraints = constraints;
			Behaviours = behaviours;
		}

		public InventoryTransaction AddItem(IItem item)
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
					return InventoryTransaction.None;
				}

				if (storedItem == null)
				{
					if (constrainedMaxSize != 0 && stackableItem.Quantity > constrainedMaxSize)
					{
						// Split stack of items into a new stack.
						var split = stackableItem.Take (constrainedMaxSize);

						storedItem = split;
						return new InventoryTransaction ()
						{
							Status = TransactionStatus.Partial,
							Items = new List<ItemTransaction> ()
							{
								new ItemTransaction()
								{
									FromInventory = null,
									ToInventory = this,
									Item = split,
									Quantity = split.Quantity
								}
							}
						};
					}
					else
					{
						// Move entire stack of items into this slot.
						storedItem = stackableItem;

						return new InventoryTransaction ()
						{
							Status = TransactionStatus.Complete,
							Items = new List<ItemTransaction> ()
							{
								new ItemTransaction()
								{
									FromInventory = null,
									ToInventory = this,
									Item = storedItem,
									Quantity = stackableItem.Quantity
								}
							}
						};
					}
				}
				else if (storedItem.Template == stackableItem.Template)
				{
					// Transfer quantities of items from one item to another.
					var currentStackableItem = (StackableItem)storedItem;

					// Adjust for pre-exisiting capacity.
					int quantityToAdd = constrainedMaxSize;
					if (constrainedMaxSize != 0)
					{
						quantityToAdd = Math.Min (constrainedMaxSize,
							Math.Min (stackableItem.Quantity, itemMaxStackSize - currentStackableItem.Quantity)
						);
					}

					if (quantityToAdd > 0)
					{
						currentStackableItem.Quantity += quantityToAdd;
						stackableItem.Quantity -= quantityToAdd;

						if (stackableItem.Quantity == 0)
						{
							return new InventoryTransaction ()
							{
								Status = TransactionStatus.Complete,
								Items = new List<ItemTransaction> ()
								{
									new ItemTransaction()
									{
										FromInventory = null,
										ToInventory = this,
										Item = currentStackableItem,
										Quantity = quantityToAdd
									}
								}
							};
						}
						else
						{
							return new InventoryTransaction ()
							{
								Status = TransactionStatus.Partial,
								Items = new List<ItemTransaction> ()
								{
									new ItemTransaction()
									{
										FromInventory = null,
										ToInventory = this,
										Item = currentStackableItem,
										Quantity = quantityToAdd
									}
								}
							};
						}
					}
					else
					{
						return InventoryTransaction.None;
					}
				}
				else
				{
					return InventoryTransaction.None;
				}
			}
			else if (item is UniqueItem uniqueItem)
			{
				if (storedItem != null)
				{
					return InventoryTransaction.None;
				}
				storedItem = uniqueItem;

				return new InventoryTransaction ()
				{
					Status = TransactionStatus.Complete,
					Items = new List<ItemTransaction> ()
					{
						new ItemTransaction()
						{
							FromInventory = null,
							ToInventory = this,
							Item = uniqueItem,
							Quantity = 1
						}
					}
				};
			}
			else
			{
				throw new InvalidOperationException ($"Item in neither a {nameof (StackableItem)} nor a {nameof (UniqueItem)}.");
			}
		}

		public InventoryTransaction DestroyItem()
		{
			if (storedItem is StackableItem stackableItem)
			{
				storedItem = null;

				return new InventoryTransaction ()
				{
					Status = TransactionStatus.Complete,
					Items = new List<ItemTransaction> ()
					{
						new ItemTransaction()
						{
							FromInventory = this,
							ToInventory = null,
							Item = stackableItem,
							Quantity = stackableItem.Quantity
						}
					}
				};
			}
			else if (storedItem is UniqueItem uniqueItem)
			{
				storedItem = null;

				return new InventoryTransaction ()
				{
					Status = TransactionStatus.Complete,
					Items = new List<ItemTransaction> ()
					{
						new ItemTransaction()
						{
							FromInventory = this,
							ToInventory = null,
							Item = uniqueItem,
							Quantity = 1
						}
					}
				};
			}
			else
			{
				throw new InvalidOperationException ($"Item in neither a {nameof (StackableItem)} nor a {nameof (UniqueItem)}.");
			}
		}

		public InventoryTransaction DragInto(IInventory other)
		{
			if (other is null)
			{
				throw new ArgumentNullException (nameof (other), $"Cannot swap into a \"null\" {nameof (IItemSlot)}.");
			}

			if (other is ItemStorageSlot itemStorageSlot)
			{
				if (storedItem == null)
				{
					return InventoryTransaction.None;
				}

				// Is item of same type?
				if (itemStorageSlot.storedItem == null ||
					storedItem.Template == itemStorageSlot.storedItem.Template)
				{
					var addResult = itemStorageSlot.AddItem (storedItem);

					if (addResult.Status == TransactionStatus.Complete)
					{
						storedItem = null;
					}

					return addResult;
				}
				else
				{
					var temp = storedItem;
					storedItem = itemStorageSlot.storedItem;
					itemStorageSlot.storedItem = temp;

					int toQuantity = 1;
					if (temp is StackableItem tempStackable)
					{
						toQuantity = tempStackable.Quantity;
					}

					return new InventoryTransaction ()
					{
						Status = TransactionStatus.Complete,
						Items = new List<ItemTransaction> ()
						{
							new ItemTransaction()
							{
								FromInventory = this,
								ToInventory = other,
								Item = itemStorageSlot.storedItem,
								Quantity = toQuantity
							}
						}
					};
				}
			}
			/*else if (other is ItemSelectSlot itemSelectSlot)
			{
				other.SetItem (StoredItem);

				return new InventoryTransaction ()
				{
					Status = TransactionStatus.Complete,
					Items = new List<ItemTransaction>()
				};
			} */
			else
			{
				throw new InvalidOperationException ($"Slot in neither a {nameof (ItemStorageSlot)} nor a {nameof (ItemSelectSlot)}.");
			}
		}

		public InventoryTransaction MoveInto(IInventory other)
		{
			if (other is null)
			{
				throw new ArgumentNullException (nameof (other), "Cannot move into a \"null\" inventory.");
			}

			if (storedItem == null)
			{
				return InventoryTransaction.None;
			}

			var result = other.AddItem (storedItem);

			if (result.Status == TransactionStatus.Complete)
			{
				storedItem = null;
			}

			return result;
		}

		public InventoryTransaction SetItem(IItem item)
		{
			if (item is null)
			{
				throw new ArgumentNullException (nameof (item), "Cannot add \"null\" item to storage slot.");
			}

			int setQuantity = 1;
			if (item is StackableItem stackableItem)
			{
				setQuantity = stackableItem.Quantity;
			}

			var inventoryTransaction = new InventoryTransaction ()
			{
				Status = TransactionStatus.Partial,
				Items = new List<ItemTransaction> ()
				{
					new ItemTransaction()
					{
						FromInventory = null,
						ToInventory = this,
						Item = item,
						Quantity = setQuantity
					}
				}
			};

			if (storedItem != null)
			{
				int previousQuantity = 1;
				if (storedItem is StackableItem stackableStoredItem)
				{
					previousQuantity = stackableStoredItem.Quantity;
				}

				inventoryTransaction.Items.Add (new ItemTransaction ()
				{
					FromInventory = this,
					ToInventory = null,
					Item = storedItem,
					Quantity = previousQuantity
				});
			}

			storedItem = item;

			return inventoryTransaction;
		}

		public InventoryTransaction Swap(IItemSlot other)
		{
			if (other is null)
			{
				throw new ArgumentNullException (nameof (other), $"Cannot swap into a \"null\" {nameof (IItemSlot)}.");
			}

			if (other is ItemStorageSlot itemStorageSlot)
			{
				var temp = storedItem;
				storedItem = itemStorageSlot.storedItem;
				itemStorageSlot.storedItem = temp;

				int toQuantity = 1;
				if (temp is StackableItem tempStackable)
				{
					toQuantity = tempStackable.Quantity;
				}

				return new InventoryTransaction ()
				{
					Status = TransactionStatus.Complete,
					Items = new List<ItemTransaction> ()
					{
						new ItemTransaction()
						{
							FromInventory = this,
							ToInventory = other,
							Item = itemStorageSlot.storedItem,
							Quantity = toQuantity
						}
					}
				};
			}
			/*else if (other is ItemSelectSlot itemSelectSlot)
			{
				other.SetItem (StoredItem);

				return new InventoryTransaction ()
				{
					Status = TransactionStatus.Complete,
					Items = new List<ItemTransaction>()
				};
			} */
			else
			{
				throw new InvalidOperationException ($"Slot in neither a {nameof (ItemStorageSlot)} nor a {nameof (ItemSelectSlot)}.");
			}
		}
	}
}
