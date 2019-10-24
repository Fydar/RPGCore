using RPGCore.Items;

namespace RPGCore.Inventory.Slots
{
	/// <summary>
	/// Constraints limit the capacity of an inventory.
	/// </summary>
	/// <example>
	/// <para>Weight inventory constraints can be used to limit the players carrying capacity by the item weight.</para>
	/// <para>Constraints can also allow only a type of item into a slot.</para>
	/// </example>
	public interface IInventoryConstraint
	{
		int QuantityCanAdd (IInventory inventory, IItem item);
	}
}
