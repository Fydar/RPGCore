using System;

namespace RPGCore.Items
{
	/// <summary>
	/// Unique items can have data stored on the single instance of the item.
	/// </summary>
	public class UniqueItem : Item
	{
		public UniqueItem(ItemTemplate template)
		{
			Template = template;
		}

		public override Item Duplicate () => throw new NotImplementedException ();
	}
}
