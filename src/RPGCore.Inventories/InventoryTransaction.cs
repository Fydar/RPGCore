using System;
using System.Collections.Generic;
using System.Text;

namespace RPGCore.Inventory.Slots
{
	/// <summary>
	/// <para>Represents a tranfer of items between inventories.</para>
	/// </summary>
	/// <remarks>
	/// <para>When a game client wants to move items from one inventory to another, they construct their desired transaction.</para>
	/// <para>A transaction can consist of adding, removing, moving or destroying items.</para>
	/// </remarks>
	public class InventoryTransaction : IEquatable<InventoryTransaction>
	{
		public static readonly InventoryTransaction None = new InventoryTransaction (TransactionStatus.None, new ItemTransaction[0]);

		public TransactionStatus Status;
		public IReadOnlyList<ItemTransaction> Items;

		public InventoryTransaction()
		{

		}

		public InventoryTransaction(TransactionStatus status, IReadOnlyList<ItemTransaction> items)
		{
			Status = status;
			Items = items;
		}

		public override bool Equals(object obj)
		{
			return Equals (obj as InventoryTransaction);
		}

		public bool Equals(InventoryTransaction other)
		{
			if (Status != other.Status)
			{
				return false;
			}

			if ((Items?.Count ?? 0) != (other.Items?.Count ?? 0))
			{
				return false;
			}

			for (int i = 0; i < Items.Count; i++)
			{
				var thisItem = Items[i];
				var otherItem = other.Items[i];

				if (thisItem != otherItem)
				{
					return false;
				}
			}

			return true;
		}

		public override int GetHashCode()
		{
			int hashCode = 2143614870;
			hashCode = hashCode * -1521134295 + Status.GetHashCode ();
			hashCode = hashCode * -1521134295 + EqualityComparer<IReadOnlyList<ItemTransaction>>.Default.GetHashCode (Items);
			return hashCode;
		}

		public override string ToString()
		{
			var sb = new StringBuilder ();

			sb.Append (nameof (InventoryTransaction));
			sb.Append ("(");
			sb.Append ("Status: ");
			sb.Append (Status.ToString ());
			sb.Append (", [");

			for (int i = 0; i < Items.Count; i++)
			{
				sb.Append (Items[i]);

				if (i != Items.Count - 1)
				{
					sb.Append (", ");
				}
			}

			sb.Append ("])");

			return sb.ToString ();
		}

		public static bool operator ==(InventoryTransaction left, InventoryTransaction right)
		{
			return EqualityComparer<InventoryTransaction>.Default.Equals (left, right);
		}

		public static bool operator !=(InventoryTransaction left, InventoryTransaction right)
		{
			return !(left == right);
		}
	}
}
