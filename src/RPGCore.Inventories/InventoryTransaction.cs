using System.Collections.Generic;

namespace RPGCore.Inventory.Slots
{
	/// <summary>
	/// <para>Represents a tranfer of items between inventories.</para>
	/// </summary>
	/// <remarks>
	/// <para>When a game client wants to move items from one inventory to another, they construct their desired transaction.</para>
	/// <para>A transaction can consist of adding, removing, moving or destroying items.</para>
	/// </remarks>
	public class InventoryTransaction
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
	}
}
