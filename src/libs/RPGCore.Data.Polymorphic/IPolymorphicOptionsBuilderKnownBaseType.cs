using System;

namespace RPGCore.Data.Polymorphic;

/// <summary>
/// Options used to configure the behaviour of a known base type.
/// </summary>
public interface IPolymorphicOptionsBuilderKnownBaseType
{
	/// <summary>
	/// Directs the serializer to locate additional sub-types automatically.
	/// </summary>
	public void UseResolvedSubTypes();

	/// <summary>
	/// Directs the serializer to locate additional sub-types automatically.
	/// </summary>
	public void UseResolvedSubTypes(Action<PolymorphicOptionsBuilderResolveSubType> options);

	/// <summary>
	/// Adds a sub-type to a <see cref="Type"/> list of valid sub-types.
	/// </summary>
	/// <param name="subType">The sub-type to allow for this <see cref="Type"/>.</param>
	void UseSubType(Type subType);

	/// <summary>
	/// Adds a sub-type to a <see cref="Type"/> list of valid sub-types.
	/// </summary>
	/// <param name="subType">The sub-type to allow for this <see cref="Type"/>.</param>
	/// <param name="options">Options used to configure how the sub-type behaves when used by this <see cref="Type"/>.</param>
	void UseSubType(Type subType, Action<PolymorphicOptionsBuilderExplicitType> options);

	/// <summary>
	/// Adds a sub-type to a <see cref="Type"/> list of valid sub-types.
	/// </summary>
	/// <typeparam name="TSubType">The sub-type to allow for this <see cref="Type"/>.</typeparam>
	/// <param name="options">Options used to configure how the sub-type behaves when used by this <see cref="Type"/>.</param>
	void UseSubType<TSubType>(Action<PolymorphicOptionsBuilderExplicitType> options);
}
