using System;
using System.Collections.Generic;

namespace RPGCore.Data.Polymorphic.Configuration
{
	/// <summary>
	/// Options for configuring how a base-type.
	/// </summary>
	public class PolymorphicConfigurationBaseType
	{
		internal readonly Dictionary<Type, PolymorphicConfigurationSubType> subTypes;

		/// <summary>
		/// The <see cref="PolymorphicConfiguration"/> this base-type configuration belongs to.
		/// </summary>
		public PolymorphicConfiguration Configuration { get; }

		/// <summary>
		/// The base-type that this <see cref="PolymorphicConfigurationBaseType"/> describes.
		/// </summary>
		public Type BaseType { get; }

		/// <summary>
		/// Configuration about all of the sub-types associated with this base-type.
		/// </summary>
		public IReadOnlyCollection<PolymorphicConfigurationSubType> SubTypes => subTypes.Values;

		internal PolymorphicConfigurationBaseType(PolymorphicConfiguration configuration, Type baseType)
		{
			Configuration = configuration;
			BaseType = baseType;
			subTypes = new Dictionary<Type, PolymorphicConfigurationSubType>();
		}

		/// <summary>
		/// Retrieves a sub-type from <see cref="SubTypes"/>.
		/// </summary>
		/// <param name="subType">The sub-type get get configuration for.</param>
		/// <returns>If the <paramref name="subType"/> has configuration associated with it, returns the <see cref="PolymorphicConfigurationSubType"/> associated with the <paramref name="subType"/>; otherwise <c>null</c>.</returns>
		public PolymorphicConfigurationSubType? GetSubTypeForType(Type subType)
		{
			subTypes.TryGetValue(subType, out var subTypeConfiguration);
			return subTypeConfiguration;
		}

		/// <summary>
		/// Retrieves a sub-type from <see cref="SubTypes"/> by a descriminator <see cref="string"/>.
		/// </summary>
		/// <param name="descriminator">A descriminator used to locate <see cref="PolymorphicConfigurationSubType"/>.</param>
		/// <returns>If the <paramref name="descriminator"/> could be associated with a <see cref="PolymorphicConfigurationSubType"/>, returns the <see cref="PolymorphicConfigurationSubType"/> associated with the <paramref name="descriminator"/>; otherwise <c>null</c>.</returns>
		public PolymorphicConfigurationSubType? GetSubTypeForDescriminator(string descriminator)
		{
			foreach (var option in subTypes.Values)
			{
				if (option.DoesDescriminatorIndicate(descriminator, Configuration.CaseInsensitive))
				{
					return option;
				}
			}
			return null;
		}

		/// <summary>
		/// Returns a <see cref="string"/> that represents the current <see cref="PolymorphicConfigurationBaseType"/>.
		/// </summary>
		/// <returns>A <see cref="string"/> that represents the current <see cref="PolymorphicConfigurationBaseType"/>.</returns>
		public override string ToString()
		{
			return BaseType.Name;
		}
	}
}
