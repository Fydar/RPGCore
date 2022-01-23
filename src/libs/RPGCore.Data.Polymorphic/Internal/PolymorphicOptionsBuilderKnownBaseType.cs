using System;
using System.Collections.Generic;

namespace RPGCore.Data.Polymorphic.Internal;

/// <summary>
/// Options for configuring a knon type for polymorphic serialization.
/// </summary>
internal class PolymorphicOptionsBuilderKnownBaseType : IPolymorphicOptionsBuilderKnownBaseType
{
	internal readonly List<IPolymorphicOptionsBuilderConfigure> configures;

	/// <summary>
	/// The type that this <see cref="PolymorphicOptionsBuilderKnownBaseType"/> represents.
	/// </summary>
	public Type Type { get; }

	internal PolymorphicOptionsBuilderKnownBaseType(Type type)
	{
		Type = type;
		configures = new List<IPolymorphicOptionsBuilderConfigure>();
	}

	/// <summary>
	/// Directs the serializer to locate additional sub-types automatically.
	/// </summary>
	public void UseResolvedSubTypes()
	{
		configures.Add(new PolymorphicOptionsBuilderConfigureResolveSubTypes());
	}

	/// <summary>
	/// Directs the serializer to locate additional sub-types automatically.
	/// </summary>
	public void UseResolvedSubTypes(Action<PolymorphicOptionsBuilderResolveSubType> options)
	{
		configures.Add(new PolymorphicOptionsBuilderConfigureResolveSubTypes(options));
	}

	/// <summary>
	/// Adds a sub-type to a <see cref="Type"/> list of valid sub-types.
	/// </summary>
	/// <param name="subType">The sub-type to allow for this <see cref="Type"/>.</param>
	public void UseSubType(Type subType)
	{
		configures.Add(new PolymorphicOptionsBuilderConfigureUseSubType(subType));
	}

	/// <summary>
	/// Adds a sub-type to a <see cref="Type"/> list of valid sub-types.
	/// </summary>
	/// <param name="subType">The sub-type to allow for this <see cref="Type"/>.</param>
	/// <param name="options">Options used to configure how the sub-type behaves when used by this <see cref="Type"/>.</param>
	public void UseSubType(Type subType, Action<PolymorphicOptionsBuilderExplicitType> options)
	{
		configures.Add(new PolymorphicOptionsBuilderConfigureUseSubType(subType, options));
	}

	/// <summary>
	/// Adds a sub-type to a <see cref="Type"/> list of valid sub-types.
	/// </summary>
	/// <typeparam name="TSubType">The sub-type to allow for this <see cref="Type"/>.</typeparam>
	/// <param name="options">Options used to configure how the sub-type behaves when used by this <see cref="Type"/>.</param>
	public void UseSubType<TSubType>(Action<PolymorphicOptionsBuilderExplicitType> options)
	{
		UseSubType(typeof(TSubType), options);
	}
}
