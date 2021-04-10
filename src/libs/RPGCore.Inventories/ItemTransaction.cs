using RPGCore.Items;
using System;
using System.Collections.Generic;

namespace RPGCore.Inventory.Slots
{
	public class ItemTransaction : IEquatable<ItemTransaction>
	{
		public IItem Item;
		public IInventory FromInventory;
		public IInventory ToInventory;
		public int Quantity;

		public ItemTransactionType Type
		{
			get
			{
				return CalculateType(FromInventory, ToInventory);
			}
		}

		private static ItemTransactionType CalculateType(IInventory from, IInventory to)
		{
			if (from == null)
			{
				if (to == null)
				{
					return ItemTransactionType.None;
				}
				else
				{
					return ItemTransactionType.Add;
				}
			}
			else
			{
				if (to == null)
				{
					return ItemTransactionType.Destroy;
				}
				else
				{
					return ItemTransactionType.Move;
				}
			}
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			if (FromInventory == null)
			{
				if (ToInventory == null)
				{
					return "None";
				}
				else
				{
					return $"Add {Item} to {ToInventory}";
				}
			}
			else
			{
				if (ToInventory == null)
				{
					return $"Destroy {Item} from {FromInventory}";
				}
				else
				{
					return $"Move {Item} from {FromInventory} to {ToInventory}";
				}
			}
		}

		/// <inheritdoc/>
		public override bool Equals(object obj)
		{
			return Equals(obj as ItemTransaction);
		}

		public bool Equals(ItemTransaction other)
		{
			return other != null &&
				   EqualityComparer<IItem>.Default.Equals(Item, other.Item) &&
				   EqualityComparer<IInventory>.Default.Equals(FromInventory, other.FromInventory) &&
				   EqualityComparer<IInventory>.Default.Equals(ToInventory, other.ToInventory) &&
				   Quantity == other.Quantity;
		}

		/// <inheritdoc/>
		public override int GetHashCode()
		{
			int hashCode = -894636067;
			hashCode = hashCode * -1521134295 + EqualityComparer<IItem>.Default.GetHashCode(Item);
			hashCode = hashCode * -1521134295 + EqualityComparer<IInventory>.Default.GetHashCode(FromInventory);
			hashCode = hashCode * -1521134295 + EqualityComparer<IInventory>.Default.GetHashCode(ToInventory);
			hashCode = hashCode * -1521134295 + Quantity.GetHashCode();
			return hashCode;
		}

		public static bool operator ==(ItemTransaction left, ItemTransaction right)
		{
			return EqualityComparer<ItemTransaction>.Default.Equals(left, right);
		}

		public static bool operator !=(ItemTransaction left, ItemTransaction right)
		{
			return !(left == right);
		}
	}
}
