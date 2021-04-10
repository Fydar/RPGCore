using RPGCore.Items;

namespace RPGCore.Inventory.Slots
{
	/// <summary>
	/// <para>Constraints limit the capacity of an inventory.</para>
	/// </summary>
	/// <example>
	/// <para>Weight inventory constraints can be used to limit the players carrying capacity by the item weight.</para>
	/// <para>Constraints can also allow only a type of item into a slot.</para>
	/// </example>
	public interface IInventoryConstraint
	{
		/// <summary>
		/// <para>Determins the maximum that this constraint will allow.</para>
		/// </summary>
		/// <param name="inventory">The inventory the item will be added to.</param>
		/// <param name="item">The item to add to the <paramref name="inventory"/>.</param>
		/// <returns>The quantity of items that can be added to the inventory.</returns>
		int QuantityCanAdd(IInventory inventory, IItem item);
	}
}
