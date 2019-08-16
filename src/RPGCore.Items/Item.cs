using System;

namespace RPGCore.Items
{
	public abstract class Item
	{
		public ItemTemplate Template { get; set; }

		/// <summary>
		/// Creates a duplicate of this item.
		/// </summary>
		/// <returns>A duplicate of this item.</returns>
		public abstract Item Duplicate ();
	}
}
