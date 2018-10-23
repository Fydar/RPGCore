using System;

namespace RPGCore
{
	public class CollectionTypeAttribute : Attribute
	{
		public Type collectionType;

		public CollectionTypeAttribute (Type type)
		{
			collectionType = type;
		}
	}
}