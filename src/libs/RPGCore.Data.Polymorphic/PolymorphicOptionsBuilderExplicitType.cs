using System;
using System.Collections.Generic;

namespace RPGCore.Data.Polymorphic;

/// <summary>
/// Options for configuring how a sub-type is identified in a base-type.
/// </summary>
public class PolymorphicOptionsBuilderExplicitType
{
	/// <summary>
	/// The base-type that thos <see cref="PolymorphicOptionsBuilderExplicitType"/> is scoped to.
	/// </summary>
	public Type BaseType { get; }

	/// <summary>
	/// The sub-type that this <see cref="PolymorphicOptionsBuilderExplicitType"/> represents.
	/// </summary>
	public Type SubType { get; }

	/// <summary>
	/// The primary alias that is used to identify this <see cref="SubType"/> in the given <see cref="BaseType"/>.
	/// <para>The default value for this property is <c>null</c>.</para>
	/// <para>If this value is when a <see cref="PolymorphicOptions"/> is constructed and this value is <c>null</c>, <see cref="PolymorphicOptionsBuilder.DefaultNamingConvention"/> is used.</para>
	/// </summary>
	public string? Descriminator { get; set; }

	/// <summary>
	/// Aliases for this <see cref="SubType"/> in the given <see cref="BaseType"/> that as are also acceptable to indicate this type.
	/// </summary>
	public List<string> Aliases { get; set; }

	internal PolymorphicOptionsBuilderExplicitType(Type baseType, Type subType)
	{
		BaseType = baseType;
		SubType = subType;
		Aliases = new List<string>();
	}
}
