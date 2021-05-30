using System;

namespace RPGCore.Data.Polymorphic.Naming
{
	public class TypeNameNamingConvention : ITypeNamingConvention
	{
		public static TypeNameNamingConvention Instance { get; } = new();

		public string GetNameForType(Type type)
		{
			return type.Name;
		}
	}
}
