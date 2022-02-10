using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Data.Polymorphic;

/// <summary>
/// Options for configuring how a base-type.
/// </summary>
public sealed class PolymorphicOptionsBaseType
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	internal readonly Dictionary<Type, PolymorphicOptionsSubType> subTypes;

	/// <summary>
	/// The <see cref="PolymorphicOptions"/> these base-type options belongs to.
	/// </summary>
	public PolymorphicOptions Options { get; }

	/// <summary>
	/// The base-type that this <see cref="PolymorphicOptionsBaseType"/> describes.
	/// </summary>
	public Type BaseType { get; }

	/// <summary>
	/// Options about all of the sub-types associated with this base-type.
	/// </summary>
	public IReadOnlyCollection<PolymorphicOptionsSubType> SubTypes => subTypes.Values;

	internal PolymorphicOptionsBaseType(PolymorphicOptions options, Type baseType)
	{
		Options = options;
		BaseType = baseType;
		subTypes = new Dictionary<Type, PolymorphicOptionsSubType>();
	}

	/// <summary>
	/// Retrieves a sub-type from <see cref="SubTypes"/>.
	/// </summary>
	/// <param name="subType">The sub-type get get configuration for.</param>
	/// <returns>If the <paramref name="subType"/> has configuration associated with it, returns the <see cref="PolymorphicOptionsSubType"/> associated with the <paramref name="subType"/>; otherwise <c>null</c>.</returns>
	public PolymorphicOptionsSubType? GetSubTypeForType(Type subType)
	{
		subTypes.TryGetValue(subType, out var subTypeOptions);
		return subTypeOptions;
	}

	/// <summary>
	/// Retrieves a sub-type from <see cref="SubTypes"/> by a descriminator <see cref="string"/>.
	/// </summary>
	/// <param name="descriminator">A descriminator used to locate <see cref="PolymorphicOptionsSubType"/>.</param>
	/// <returns>If the <paramref name="descriminator"/> could be associated with a <see cref="PolymorphicOptionsSubType"/>, returns the <see cref="PolymorphicOptionsSubType"/> associated with the <paramref name="descriminator"/>; otherwise <c>null</c>.</returns>
	public PolymorphicOptionsSubType? GetSubTypeForDescriminator(string descriminator)
	{
		foreach (var option in subTypes.Values)
		{
			if (option.DoesDescriminatorIndicate(descriminator, Options.CaseInsensitive))
			{
				return option;
			}
		}
		return null;
	}

	/// <summary>
	/// Returns a <see cref="string"/> that represents the current <see cref="PolymorphicOptionsBaseType"/>.
	/// </summary>
	/// <returns>A <see cref="string"/> that represents the current <see cref="PolymorphicOptionsBaseType"/>.</returns>
	public override string ToString()
	{
		return BaseType.Name;
	}
}
