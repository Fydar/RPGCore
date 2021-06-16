using RPGCore.Data.Polymorphic.Configuration;
using RPGCore.Data.Polymorphic.Naming;
using System;
using System.Collections.Generic;

namespace RPGCore.Data.Polymorphic
{
	public class PolymorphicOptions
	{
		/// <summary>
		/// Determines the name of the field that is used to determine polymorphic types.
		/// </summary>
		public string DescriminatorName { get; set; } = "$type";

		/// <summary>
		/// Determines whether type names should be case-insensitive.
		/// </summary>
		public bool CaseInsensitive { get; set; } = true;

		/// <summary>
		/// Determines the default type name to use when none is supplied.
		/// <para>This property by default uses an instance of <see cref="TypeFullnameNamingConvention"/>.</para>
		/// </summary>
		public ITypeNamingConvention DefaultNamingConvention { get; set; }

		/// <summary>
		/// Determines the default type alias conventions to use when none is supplied.
		/// <para>This property has a default value of <c>null</c>.</para>
		/// </summary>
		public ITypeNamingConvention[]? DefaultAliasConventions { get; set; } = null;

		internal Dictionary<Type, PolymorphicOptionsBaseType> knownBaseTypes = new();
		internal Dictionary<Type, PolymorphicOptionsSubType> knownSubTypes = new();

		public PolymorphicOptions()
		{
			DefaultNamingConvention = TypeFullnameNamingConvention.Instance;
		}

		public PolymorphicOptions UseKnownBaseType(Type knownBaseType, Action<PolymorphicOptionsBaseType> options)
		{
			if (!knownBaseTypes.TryGetValue(knownBaseType, out var typeInfo))
			{
				typeInfo = new PolymorphicOptionsBaseType(knownBaseType);
				knownBaseTypes.Add(knownBaseType, typeInfo);
			}

			options.Invoke(typeInfo);
			return this;
		}

		public PolymorphicOptions UseKnownSubType(Type knownSubType, Action<PolymorphicOptionsSubType> options)
		{
			if (!knownSubTypes.TryGetValue(knownSubType, out var typeInfo))
			{
				typeInfo = new PolymorphicOptionsSubType(knownSubType);
				knownSubTypes.Add(knownSubType, typeInfo);
			}

			options.Invoke(typeInfo);
			return this;
		}

		public PolymorphicConfiguration Build()
		{
			return new PolymorphicConfiguration(this);
		}
	}
}
