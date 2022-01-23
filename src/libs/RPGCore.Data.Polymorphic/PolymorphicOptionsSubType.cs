using System;

namespace RPGCore.Data.Polymorphic;

/// <summary>
/// Information about the sub-type scoped to a base-type.
/// </summary>
public sealed class PolymorphicOptionsSubType
{
	/// <summary>
	/// The <see cref="PolymorphicOptionsBaseType"/> these sub-type options is scoped to.
	/// </summary>
	public PolymorphicOptionsBaseType BaseType { get; }

	/// <summary>
	/// The type that is associated with this <see cref="PolymorphicOptionsSubType"/>.
	/// </summary>
	public Type Type { get; }

	/// <summary>
	/// A <see cref="string"/> identifier associated with the <see cref="Type"/>.
	/// </summary>
	public string Name { get; internal set; }

	/// <summary>
	/// A collection of <see cref="string"/> aliases associated with the <see cref="Type"/>.
	/// </summary>
	public string[] Aliases { get; internal set; }

	/// <summary>
	/// The <see cref="PolymorphicOptions"/> these sub-type options belongs to.
	/// </summary>
	public PolymorphicOptions Options => BaseType.Options;

	internal PolymorphicOptionsSubType(
		PolymorphicOptionsBaseType baseType,
		Type type)
	{
		BaseType = baseType;
		Type = type;
		Name = string.Empty;
		Aliases = Array.Empty<string>();
	}

	/// <summary>
	/// Determines whether this <see cref="PolymorphicOptionsSubType"/> matches the <paramref name="descriminator"/>.
	/// </summary>
	/// <param name="descriminator">A descriminator to test against this <see cref="PolymorphicOptionsSubType"/>.</param>
	/// <param name="caseInsentitive">Whether case-insensitive string-comparison should be used.</param>
	/// <returns><c>true</c> if the <paramref name="descriminator"/> matches this <see cref="PolymorphicOptionsSubType"/>.</returns>
	public bool DoesDescriminatorIndicate(string descriminator, bool caseInsentitive)
	{
		var comparison = caseInsentitive
			? StringComparison.OrdinalIgnoreCase
			: StringComparison.Ordinal;

		if (string.Equals(Name, descriminator, comparison))
		{
			return true;
		}

		foreach (string alias in Aliases)
		{
			if (string.Equals(alias, descriminator, comparison))
			{
				return true;
			}
		}
		return false;
	}

	/// <summary>
	/// Returns a <see cref="string"/> that represents the current <see cref="PolymorphicOptionsSubType"/>.
	/// </summary>
	/// <returns>A <see cref="string"/> that represents the current <see cref="PolymorphicOptionsSubType"/>.</returns>
	public override string ToString()
	{
		return Name;
	}
}
