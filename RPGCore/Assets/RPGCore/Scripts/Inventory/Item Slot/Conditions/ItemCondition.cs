using RPGCore;
using System.Collections.Generic;

namespace RPGCore.Inventories
{
	public abstract class ItemCondition
	{
		public abstract bool IsValid (ItemSurrogate item);
	}

	public static class ItemConditionExtensions
	{
		public static bool IsValid (this IEnumerable<ItemCondition> conditions, ItemSurrogate item)
		{
			if (conditions == null)
				return true;

			foreach (ItemCondition condition in conditions)
			{
				if (!condition.IsValid (item))
					return false;
			}

			return true;
		}
	}
}