using RPGCore.Data.Polymorphic.Configuration;
using RPGCore.Data.Polymorphic.Naming;
using System;
using System.Collections.Generic;

namespace RPGCore.Data.Polymorphic
{
	/// <summary>
	/// Options used to configure polymorphic serialization.
	/// </summary>
	public class PolymorphicOptions
	{
		internal Dictionary<Type, PolymorphicOptionsBaseType> knownBaseTypes = new();
		internal Dictionary<Type, PolymorphicOptionsSubType> knownSubTypes = new();

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
		/// <para>This property by default uses an instance of <see cref="TypeFullNameNamingConvention"/>.</para>
		/// </summary>
		public ITypeNamingConvention DefaultNamingConvention { get; set; }

		/// <summary>
		/// Determines the default type alias conventions to use when none is supplied.
		/// <para>This property has a default value of <c>null</c>.</para>
		/// </summary>
		public ITypeNamingConvention[]? DefaultAliasConventions { get; set; } = null;

		/// <summary>
		/// Creates a new instance of this <see cref="PolymorphicOptions"/> class.
		/// </summary>
		public PolymorphicOptions()
		{
			DefaultNamingConvention = TypeFullNameNamingConvention.Instance;
		}

		/// <summary>
		/// Registers a known base-type to this <see cref="PolymorphicOptions"/>.
		/// </summary>
		/// <param name="knownBaseType">The known base-type to add.</param>
		/// <param name="options">Options assoociated with this known base-type.</param>
		/// <returns>This <see cref="PolymorphicOptions"/> for continued configuration.</returns>
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

		/// <summary>
		/// Registers a known sub-type to this <see cref="PolymorphicOptions"/>.
		/// </summary>
		/// <param name="knownSubType">The known sub-type to add.</param>
		/// <param name="options">Options assoociated with this known sub-type.</param>
		/// <returns>This <see cref="PolymorphicOptions"/> for continued configuration.</returns>
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

		/// <summary>
		/// Constructs a new <see cref="PolymorphicConfiguration"/> from the current state of this <see cref="PolymorphicOptions"/>.
		/// </summary>
		/// <returns>A <see cref="PolymorphicConfiguration"/> from the current state of the this <see cref="PolymorphicOptions"/>.</returns>
		public PolymorphicConfiguration Build()
		{
			return new PolymorphicConfiguration(this);
		}
	}
}
