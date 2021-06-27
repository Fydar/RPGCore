using System;

namespace RPGCore.Data.Polymorphic.Naming
{
	/// <summary>
	/// Generates a <see cref="string"/> name for a <see cref="Type"/>.
	/// </summary>
	public interface ITypeNamingConvention
	{
		/// <summary>
		/// Generates a <see cref="string"/> name for a <paramref name="type"/>.
		/// </summary>
		/// <param name="type">The <see cref="Type"/> to generate a name for.</param>
		/// <returns>A <see cref="string"/> name for the supplied <paramref name="type"/>.</returns>
		string GetNameForType(Type type);
	}
}
