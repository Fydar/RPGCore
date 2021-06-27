using System;
using System.Collections.Generic;

namespace RPGCore.Data.Polymorphic
{
	/// <summary>
	/// Options for how a sub-type should be identified in serialization.
	/// </summary>
	public class PolymorphicOptionsSubType
	{
		internal PolymorphicOptionsResolveBaseType? resolveBaseTypeOptions;

		/// <summary>
		/// The type that this <see cref="PolymorphicOptionsSubType"/> represents.
		/// </summary>
		public Type SubType { get; }

		/// <summary>
		/// The primary alias for this subtype that is used to identify this sub-type.
		/// </summary>
		public string? Descriminator { get; set; }

		/// <summary>
		/// Aliases for this subtype that as are also acceptable to indicate this sub-type.
		/// </summary>
		public List<string> Aliases { get; set; }

		/// <summary>
		/// A collection of base-types that are explicitly declared as usable by this sub-type.
		/// </summary>
		internal Dictionary<Type, PolymorphicOptionsSubTypeBaseType> knownBaseTypes;

		internal PolymorphicOptionsSubType(Type subType)
		{
			SubType = subType;
			Aliases = new List<string>();
			knownBaseTypes = new Dictionary<Type, PolymorphicOptionsSubTypeBaseType>();
		}

		/// <summary>
		/// Directs the serializer to locate additional base-types automatically.
		/// </summary>
		public void ResolveBaseTypesAutomatically()
		{
			if (resolveBaseTypeOptions == null)
			{
				resolveBaseTypeOptions = new PolymorphicOptionsResolveBaseType();
			}
		}

		/// <summary>
		/// Directs the serializer to locate additional base-types automatically.
		/// </summary>
		public void ResolveBaseTypesAutomatically(Action<PolymorphicOptionsResolveBaseType> options)
		{
			if (resolveBaseTypeOptions == null)
			{
				resolveBaseTypeOptions = new PolymorphicOptionsResolveBaseType();
			}

			options?.Invoke(resolveBaseTypeOptions);
		}

		/// <summary>
		/// Adds a base-type to a <see cref="SubType"/> list of valid base-types.
		/// </summary>
		/// <param name="baseType">The base-type to allow for this <see cref="SubType"/>.</param>
		public void UseBaseType(Type baseType)
		{
			AddBaseTypeToThisSubType(baseType);
		}

		/// <summary>
		/// Adds a base-type to a <see cref="SubType"/> list of valid base-types.
		/// </summary>
		/// <param name="baseType">The base-type to allow for this <see cref="SubType"/>.</param>
		/// <param name="options">Options used to configure how the base-type behaves when used by this <see cref="SubType"/>.</param>
		public void UseBaseType(Type baseType, Action<PolymorphicOptionsSubTypeBaseType> options)
		{
			var typeInfo = AddBaseTypeToThisSubType(baseType);

			options.Invoke(typeInfo);
		}

		private PolymorphicOptionsSubTypeBaseType AddBaseTypeToThisSubType(Type baseType)
		{
			if (!knownBaseTypes.TryGetValue(baseType, out var typeInfo))
			{
				typeInfo = new PolymorphicOptionsSubTypeBaseType(baseType);
				knownBaseTypes[baseType] = typeInfo;
			}

			return typeInfo;
		}
	}
}
