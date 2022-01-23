using RPGCore.Items;

namespace RPGCore.Inventory.Slots;

public interface IItemSlot : IInventory
{
	/// <summary>
	/// <para>The current item in the slot.</para>
	/// </summary>
	IItem CurrentItem { get; }

	/// <summary>
	/// <para>Destroys the item currently contained within this slot.</para>
	/// </summary>
	/// <returns>An <see cref="InventoryTransaction"/> that represents the results of the destroy.</returns>
	InventoryTransaction DestroyItem();

	/// <summary>
	/// <para>Attempts to move an item from this slot into another inventory.</para>
	/// <list type="bullet">
	/// <item>If the target is a slot then swapping may occur.</item>
	/// <item>If the target is not at capacity, then items can be transfered from this slot into the target.</item>
	/// </list>
	/// </summary>
	/// <example>
	/// <para>This is used when the player drags from one slot into another.</para>
	/// </example>
	/// <param name="other"></param>
	/// <returns>An <see cref="InventoryTransaction"/> that represents the results of the drag.</returns>
	InventoryTransaction DragInto(IInventory other);

	/// <summary>
	/// <para>Attempts to move an item from the this slot into another inventory.</para>
	/// </summary>
	/// <param name="other">The inventory to move the contents of this slot into.</param>
	/// <returns>An <see cref="InventoryTransaction"/> that represents the results of the move.</returns>
	/// <example>
	/// <para>This is used when a player shift-clicks an item from one inventory and a target inventory is defined.</para>
	/// </example>
	InventoryTransaction MoveInto(IInventory other);

	/// <summary>
	/// <para>Forcefully sets the item that's contained within the current slot.</para>
	/// </summary>
	/// <param name="item"></param>
	/// <returns>An <see cref="InventoryTransaction"/> that represents the results of the setting.</returns>
	InventoryTransaction SetItem(IItem item);

	/// <summary>
	/// <para>Attempts to swap the contents of this slot with another.</para>
	/// <para>No partial swapping can occour.</para>
	/// </summary>
	/// <param name="other"></param>
	/// <returns></returns>
	/// <returns>An <see cref="InventoryTransaction"/> that represents the results of the drag.</returns>
	InventoryTransaction Swap(IItemSlot other);
}
