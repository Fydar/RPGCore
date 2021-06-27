using System;

namespace RPGCore.Data.Polymorphic.Configuration
{
	/// <summary>
	/// Information about the sub-type scoped to a base-type.
	/// </summary>
	public class PolymorphicConfigurationSubType
	{
		/// <summary>
		/// The <see cref="PolymorphicConfigurationBaseType"/> this sub-type configuration is scoped to.
		/// </summary>
		public PolymorphicConfigurationBaseType BaseType { get; }

		/// <summary>
		/// The type that is associated with this <see cref="PolymorphicConfigurationSubType"/>.
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
		/// The <see cref="PolymorphicConfiguration"/> this sub-type configuration belongs to.
		/// </summary>
		public PolymorphicConfiguration Configuration => BaseType.Configuration;

		internal PolymorphicConfigurationSubType(
			PolymorphicConfigurationBaseType baseType,
			Type type)
		{
			BaseType = baseType;
			Type = type;
			Name = type.FullName;
			Aliases = Array.Empty<string>();
		}

		/// <summary>
		/// Determines whether this <see cref="PolymorphicConfigurationSubType"/> matches the <paramref name="descriminator"/>.
		/// </summary>
		/// <param name="descriminator">A descriminator to test against this <see cref="PolymorphicConfigurationSubType"/>.</param>
		/// <param name="caseInsentitive">Whether case-insensitive string-comparison should be used.</param>
		/// <returns><c>true</c> if the <paramref name="descriminator"/> matches this <see cref="PolymorphicConfigurationSubType"/>.</returns>
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
		/// Returns a <see cref="string"/> that represents the current <see cref="PolymorphicConfigurationSubType"/>.
		/// </summary>
		/// <returns>A <see cref="string"/> that represents the current <see cref="PolymorphicConfigurationSubType"/>.</returns>
		public override string ToString()
		{
			return Name;
		}
	}
}
