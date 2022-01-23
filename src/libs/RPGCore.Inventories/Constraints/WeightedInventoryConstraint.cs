using RPGCore.Items;
using System;

namespace RPGCore.Inventory.Slots;

/// <summary>
/// <para>Constraints limit the capacity of an inventory.</para>
/// </summary>
/// <example>
/// <para>Weight inventory constraints can be used to limit the players carrying capacity by the item weight.</para>
/// <para>Constraints can also allow only a type of item into a slot.</para>
/// </example>
public class WeightedInventoryConstraint : IInventoryConstraint
{
	public int Weight;

	public WeightedInventoryConstraint(int weight)
	{
		Weight = weight;
	}

	/// <inheritdoc/>
	public int QuantityCanAdd(IInventory inventory, IItem item)
	{
		if (item.Template.Weight == 0)
		{
			return int.MaxValue;
		}

		int currentWeight = 0;
		foreach (var otherItem in inventory.Items)
		{
			if (otherItem is StackableItem stackableItem)
			{
				currentWeight += otherItem.Template.Weight * stackableItem.Quantity;
			}
			else
			{
				currentWeight += otherItem.Template.Weight;
			}
		}

		int remainingWeight = Math.Max(Weight - currentWeight, 0);

		int quantityCanAdd = remainingWeight / item.Template.Weight;
		return quantityCanAdd;
	}

	/// <inheritdoc/>
	public override string ToString()
	{
		return $"Weight Constraint of {Weight}";
	}
}
