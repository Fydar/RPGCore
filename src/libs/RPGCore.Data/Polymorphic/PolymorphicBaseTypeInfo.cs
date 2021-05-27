using System;
using System.Collections.Generic;

namespace RPGCore.Data.Polymorphic
{
	public class PolymorphicBaseTypeInfo
	{
		public Type BaseType { get; }
		public List<Type> KnownSubtypes { get; set; }

		public PolymorphicBaseTypeInfo(Type baseType)
		{
			BaseType = baseType;
			KnownSubtypes = new List<Type>();
		}
	}
}
