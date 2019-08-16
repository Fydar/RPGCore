using System;

namespace RPGCore.Items
{
	/// <summary>
	/// Stackable items have a quantity and all behaviours can't store data on a single item.
	/// </summary>
	public class StackableItem : Item
	{
		public int Quantity { get; set; }

		public StackableItem(ItemTemplate template, int quantity)
		{
			Template = template;
			Quantity = quantity;
		}

		/// <summary>
		/// Take a couple of items from this <see cref="StackableItem"/> in order to split them into
		/// seperate stacks.
		/// </summary>
		/// <param name="quantity">The quantity of items to take from this <see cref="StackableItem"/>.</param>
		/// <returns>A <see cref="StackableItem"/> with the quantity of items taken from this item.</returns>
		/// <exception cref="InvalidOperationException">Thrown when <paramref name="quantity"/> is greater than or equal to the <see cref="Quantity"/> of this <see cref="StackableItem"/>.</exception>
		public StackableItem Take (int quantity)
		{
			if (quantity > Quantity)
			{
				throw new InvalidOperationException ($"Can't take more items from a {nameof (StackableItem)} than there are in the stack.");
			}

			if (quantity == Quantity)
			{
				throw new InvalidOperationException ($"Instead of taking the whole stack from a {nameof (StackableItem)}, move the whole {nameof (StackableItem)}.");
			}

			Quantity -= quantity;

			return new StackableItem (Template, quantity);
		}

		/// <summary>
		/// Creates a duplicate of this item.
		/// </summary>
		/// <returns>A duplicate of this item.</returns>
		public override Item Duplicate ()
		{
			return new StackableItem (Template, Quantity);
		}
	}
}
