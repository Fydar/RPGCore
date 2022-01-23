using RPGCore.Items;
using System.Collections.Generic;

namespace RPGCore.Inventory.Slots;

/// <summary>
/// <para>An inventory that contains <see cref="IItem"/>s.</para>
/// </summary>
public interface IInventory
{
	/// <summary>
	/// <para>The <see cref="IInventory"/> containing this <see cref="IInventory"/>.</para>
	/// </summary>
	IInventory Parent { get; }

	/// <summary>
	/// <para>All items contained within this inventory.</para>
	/// </summary>
	IEnumerable<IItem> Items { get; }

	/// <summary>
	/// <para>Add an <see cref="IItem"/> to the inventory.</para>
	/// </summary>
	/// <param name="item">The <see cref="IItem"/> to add to the inventory.</param>
	/// <returns>An inventory transaction representing the result of the item adding.</returns>
	InventoryTransaction AddItem(IItem item);
}
