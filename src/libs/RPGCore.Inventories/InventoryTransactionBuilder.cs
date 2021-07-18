using System.Collections;
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
	public class InventoryTransactionBuilder : IEnumerable<ItemTransaction>
	{
		private readonly List<ItemTransaction> items;

		public InventoryTransactionBuilder()
		{
			items = new List<ItemTransaction>();
		}

		public void Add(ItemTransaction itemTransaction)
		{
			items.Add(itemTransaction);
		}

		public InventoryTransaction Build(TransactionStatus status)
		{
			return new InventoryTransaction(status, items);
		}

		public IEnumerator<ItemTransaction> GetEnumerator()
		{
			return items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return items.GetEnumerator();
		}
	}
}
