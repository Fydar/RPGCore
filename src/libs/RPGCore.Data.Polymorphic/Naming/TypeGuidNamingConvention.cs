﻿using System;

namespace RPGCore.Data.Polymorphic.Naming;

/// <summary>
/// Generates a <see cref="string"/> name for a <see cref="Type"/> in the format of the types <see cref="Type.GUID"/>.
/// </summary>
public class TypeGuidNamingConvention : ITypeNamingConvention
{
	/// <summary>
	/// An instance of the <see cref="TypeAssemblyQualifiedNameNamingConvention"/> naming convention.
	/// </summary>
	public static TypeGuidNamingConvention Instance { get; } = new();

	/// <summary>
	/// Generates a <see cref="string"/> name for a <paramref name="type"/> in the format of the types <see cref="Type.GUID"/>.
	/// </summary>
	/// <param name="type">The <see cref="Type"/> to generate a name for.</param>
	/// <returns>A <see cref="string"/> in the format of the <paramref name="type"/> <see cref="Type.GUID"/>.</returns>
	public string GetNameForType(Type type)
	{
		return type.GUID.ToString();
	}
}
