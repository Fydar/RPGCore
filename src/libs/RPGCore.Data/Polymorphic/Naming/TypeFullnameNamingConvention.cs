using System;

namespace RPGCore.Data.Polymorphic.Naming
{
	public class TypeFullnameNamingConvention : ITypeNamingConvention
	{
		public static TypeFullnameNamingConvention Instance { get; } = new();

		public string GetNameForType(Type type)
		{
			return type.FullName;
		}
	}
}
