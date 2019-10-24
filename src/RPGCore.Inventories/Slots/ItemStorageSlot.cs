using RPGCore.Items;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Inventory.Slots
{
	public class ItemStorageSlot : IItemSlot
	{
		public IInventoryBehaviour[] Behaviours { get; }
		public IInventoryConstraint[] Constraints { get; }
		public IItem CurrentItem => StoredItem;

		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		public IEnumerable<IItem> Items
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

		public int MaxStackSize { get; set; }

		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		internal IItem StoredItem;

		public ItemStorageSlot (IInventoryConstraint[] constraints = null, IInventoryBehaviour[] behaviours = null)
		{
			Constraints = constraints;
			Behaviours = behaviours;
		}

		public InventoryTransaction AddItem (IItem item)
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

				if (StoredItem == null)
				{
					if (constrainedMaxSize != 0 && stackableItem.Quantity > constrainedMaxSize)
					{
						// Split stack of items into a new stack.
						var split = stackableItem.Take (constrainedMaxSize);

						StoredItem = split;
						return new InventoryTransaction ()
						{
							Status = TransactionStatus.Partial,
							Items = new List<ItemTransaction> ()
							{
								new ItemTransaction()
								{
									FromInventory = null,
									ToInventory = this,
									Type = ItemTransactionType.Add,
									Item = split,
									Quantity = split.Quantity
								}
							}
						};
					}
					else
					{
						// Move entire stack of items into this slot.
						StoredItem = stackableItem;

						return new InventoryTransaction ()
						{
							Status = TransactionStatus.Complete,
							Items = new List<ItemTransaction> ()
							{
								new ItemTransaction()
								{
									FromInventory = null,
									ToInventory = this,
									Type = ItemTransactionType.Add,
									Item = StoredItem,
									Quantity = stackableItem.Quantity
								}
							}
						};
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
										Type = ItemTransactionType.Add,
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
										Type = ItemTransactionType.Add,
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
				if (StoredItem != null)
				{
					return InventoryTransaction.None;
				}
				StoredItem = uniqueItem;

				return new InventoryTransaction ()
				{
					Status = TransactionStatus.Complete,
					Items = new List<ItemTransaction> ()
					{
						new ItemTransaction()
						{
							FromInventory = null,
							ToInventory = this,
							Type = ItemTransactionType.Add,
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

		public InventoryTransaction DestroyItem ()
		{
			if (StoredItem is StackableItem stackableItem)
			{
				StoredItem = null;

				return new InventoryTransaction ()
				{
					Status = TransactionStatus.Complete,
					Items = new List<ItemTransaction> ()
					{
						new ItemTransaction()
						{
							FromInventory = null,
							ToInventory = this,
							Type = ItemTransactionType.Destroy,
							Item = stackableItem,
							Quantity = stackableItem.Quantity
						}
					}
				};
			}
			else if (StoredItem is UniqueItem uniqueItem)
			{
				StoredItem = null;

				return new InventoryTransaction ()
				{
					Status = TransactionStatus.Complete,
					Items = new List<ItemTransaction> ()
					{
						new ItemTransaction()
						{
							FromInventory = null,
							ToInventory = this,
							Type = ItemTransactionType.Destroy,
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

		public InventoryTransaction DragInto (IInventory other)
		{
			if (other is null)
			{
				throw new ArgumentNullException (nameof (other), $"Cannot swap into a \"null\" {nameof (IItemSlot)}.");
			}

			if (other is ItemStorageSlot itemStorageSlot)
			{
				if (StoredItem == null)
				{
					return InventoryTransaction.None;
				}

				// Is item of same type?
				if (itemStorageSlot.StoredItem == null ||
					StoredItem.Template == itemStorageSlot.StoredItem.Template)
				{
					var addResult = itemStorageSlot.AddItem (StoredItem);

					if (addResult.Status == TransactionStatus.Complete)
					{
						StoredItem = null;
					}

					return addResult;
				}
				else
				{
					var temp = StoredItem;
					StoredItem = itemStorageSlot.StoredItem;
					itemStorageSlot.StoredItem = temp;

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
								Type = ItemTransactionType.Move,
								Item = itemStorageSlot.StoredItem,
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

		public InventoryTransaction MoveInto (IInventory other)
		{
			if (other is null)
			{
				throw new ArgumentNullException (nameof (other), "Cannot move into a \"null\" inventory.");
			}

			var result = other.AddItem (StoredItem);

			if (result.Status == TransactionStatus.Complete)
			{
				StoredItem = null;
			}

			return result;
		}

		public InventoryTransaction SetItem (IItem item)
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
						Type = ItemTransactionType.Add,
						Item = item,
						Quantity = setQuantity
					}
				}
			};

			if (StoredItem != null)
			{
				int previousQuantity = 1;
				if (StoredItem is StackableItem stackableStoredItem)
				{
					previousQuantity = stackableStoredItem.Quantity;
				}

				inventoryTransaction.Items.Add (new ItemTransaction ()
				{
					Type = ItemTransactionType.Destroy,
					Item = StoredItem,
					Quantity = previousQuantity
				});
			}

			StoredItem = item;

			return inventoryTransaction;
		}

		public InventoryTransaction Swap (IItemSlot other)
		{
			if (other is null)
			{
				throw new ArgumentNullException (nameof (other), $"Cannot swap into a \"null\" {nameof (IItemSlot)}.");
			}

			if (other is ItemStorageSlot itemStorageSlot)
			{
				var temp = StoredItem;
				StoredItem = itemStorageSlot.StoredItem;
				itemStorageSlot.StoredItem = temp;

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
							Type = ItemTransactionType.Move,
							Item = itemStorageSlot.StoredItem,
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
