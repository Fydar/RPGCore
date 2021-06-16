using System;

namespace RPGCore.Data.Polymorphic.Naming
{
	public class TypeAssemblyQualifiedNameNamingConvention : ITypeNamingConvention
	{
		public static TypeAssemblyQualifiedNameNamingConvention Instance { get; } = new();

		public string GetNameForType(Type type)
		{
			var assemblyName = type.Assembly.GetName();
			return $"{type.FullName}, {assemblyName.Name}";
		}
	}
}
