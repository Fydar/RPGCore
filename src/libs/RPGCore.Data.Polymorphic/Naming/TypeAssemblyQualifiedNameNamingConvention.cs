using System;

namespace RPGCore.Data.Polymorphic.Naming
{
	/// <summary>
	/// Generates a <see cref="string"/> name for a <see cref="Type"/> in the format of the types assembly qualified name.
	/// </summary>
	public class TypeAssemblyQualifiedNameNamingConvention : ITypeNamingConvention
	{
		/// <summary>
		/// An instance of the <see cref="TypeAssemblyQualifiedNameNamingConvention"/> naming convention.
		/// </summary>
		public static TypeAssemblyQualifiedNameNamingConvention Instance { get; } = new();

		/// <summary>
		/// Generates a <see cref="string"/> name for a <paramref name="type"/> in the format of the types assembly qualified name.
		/// </summary>
		/// <param name="type">The <see cref="Type"/> to generate a name for.</param>
		/// <returns>A <see cref="string"/> in the format of the <paramref name="type"/> assembly qualified name.</returns>
		public string GetNameForType(Type type)
		{
			var assemblyName = type.Assembly.GetName();
			return $"{type.FullName}, {assemblyName.Name}";
		}
	}
}
