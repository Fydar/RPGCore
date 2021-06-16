using System;

namespace RPGCore.Data.Polymorphic.Naming
{
	public class TypeGuidNamingConvention : ITypeNamingConvention
	{
		public static TypeGuidNamingConvention Instance { get; } = new();

		public string GetNameForType(Type type)
		{
			return type.GUID.ToString();
		}
	}
}
