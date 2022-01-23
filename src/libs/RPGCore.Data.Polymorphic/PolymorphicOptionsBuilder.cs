using RPGCore.Data.Polymorphic.Internal;
using RPGCore.Data.Polymorphic.Naming;
using System;
using System.Collections.Generic;

namespace RPGCore.Data.Polymorphic;

/// <summary>
/// Options used to configure polymorphic serialization.
/// </summary>
public class PolymorphicOptionsBuilder
{
	internal List<PolymorphicOptionsBuilderKnownTypeFactory> knownTypeFactories = new();

	/// <summary>
	/// Determines the name of the property that is used to determine polymorphic types.
	/// <para>The default value for this property is <c>"$type"</c>.</para>
	/// </summary>
	public string DescriminatorName { get; set; } = "$type";

	/// <summary>
	/// Determines whether type names should be case-insensitive.
	/// <para>The default value for this property is <c>true</c>.</para>
	/// </summary>
	public bool CaseInsensitive { get; set; } = true;

	/// <summary>
	/// Determines the default type name to use when none is supplied.
	/// <para>This property by default uses an instance of <see cref="TypeFullNameNamingConvention"/>.</para>
	/// </summary>
	public ITypeNamingConvention DefaultNamingConvention { get; set; }

	/// <summary>
	/// Creates a new instance of this <see cref="PolymorphicOptionsBuilder"/> class.
	/// </summary>
	public PolymorphicOptionsBuilder()
	{
		DefaultNamingConvention = TypeFullNameNamingConvention.Instance;
	}

	/// <summary>
	/// Registers a known base-type to this <see cref="PolymorphicOptionsBuilder"/>.
	/// </summary>
	/// <param name="knownBaseType">The known base-type to add.</param>
	/// <param name="options">Options assoociated with this known base-type.</param>
	/// <returns>This <see cref="PolymorphicOptionsBuilder"/> for continued configuration.</returns>
	public PolymorphicOptionsBuilder UseKnownBaseType(Type knownBaseType, Action<IPolymorphicOptionsBuilderKnownBaseType> options)
	{
		knownTypeFactories.Add(new PolymorphicOptionsBuilderKnownTypeFactory()
		{
			type = knownBaseType,
			factory = options
		});
		return this;
	}

	/// <summary>
	/// Registers a known sub-type to this <see cref="PolymorphicOptionsBuilder"/>.
	/// </summary>
	/// <param name="knownSubType">The known sub-type to add.</param>
	/// <param name="options">Options assoociated with this known sub-type.</param>
	/// <returns>This <see cref="PolymorphicOptionsBuilder"/> for continued configuration.</returns>
	public PolymorphicOptionsBuilder UseKnownSubType(Type knownSubType, Action<IPolymorphicOptionsBuilderKnownSubType> options)
	{
		knownTypeFactories.Add(new PolymorphicOptionsBuilderKnownTypeFactory()
		{
			type = knownSubType,
			factory = options
		});
		return this;
	}

	/// <summary>
	/// Constructs a new <see cref="PolymorphicOptions"/> from the current state of this <see cref="PolymorphicOptionsBuilder"/>.
	/// </summary>
	/// <returns>A <see cref="PolymorphicOptions"/> from the current state of the this <see cref="PolymorphicOptionsBuilder"/>.</returns>
	public PolymorphicOptions Build()
	{
		return new PolymorphicOptions(this);
	}
}
