using System;
using System.Collections.Generic;

namespace RPGCore.Data.Polymorphic
{
	/// <summary>
	/// Options for configuring the polymorphic types available to the serializer.
	/// </summary>
	public class PolymorphicOptionsBaseType
	{
		internal PolymorphicOptionsResolveSubType? resolveSubTypeOptions;

		/// <summary>
		/// The type that this <see cref="PolymorphicOptionsBaseType"/> represents.
		/// </summary>
		public Type BaseType { get; }

		/// <summary>
		/// A collection of sub-types that are explicitly declared as suitable for this base-type.
		/// </summary>
		internal readonly Dictionary<Type, PolymorphicOptionsBaseTypeSubType> knownSubTypes;

		internal PolymorphicOptionsBaseType(Type baseType)
		{
			BaseType = baseType;
			knownSubTypes = new Dictionary<Type, PolymorphicOptionsBaseTypeSubType>();
		}

		/// <summary>
		/// Directs the serializer to locate additional sub-types automatically.
		/// </summary>
		public void ResolveSubTypesAutomatically()
		{
			if (resolveSubTypeOptions == null)
			{
				resolveSubTypeOptions = new PolymorphicOptionsResolveSubType();
			}
		}

		/// <summary>
		/// Directs the serializer to locate additional sub-types automatically.
		/// </summary>
		public void ResolveSubTypesAutomatically(Action<PolymorphicOptionsResolveSubType> options)
		{
			if (resolveSubTypeOptions == null)
			{
				resolveSubTypeOptions = new PolymorphicOptionsResolveSubType();
			}

			options?.Invoke(resolveSubTypeOptions);
		}

		/// <summary>
		/// Adds a sub-type to a <see cref="BaseType"/> list of valid sub-types.
		/// </summary>
		/// <param name="subType">The sub-type to allow for this <see cref="BaseType"/>.</param>
		public void UseSubType(Type subType)
		{
			AddSubTypeToThisBaseType(subType);
		}

		/// <summary>
		/// Adds a sub-type to a <see cref="BaseType"/> list of valid sub-types.
		/// </summary>
		/// <param name="subType">The sub-type to allow for this <see cref="BaseType"/>.</param>
		/// <param name="options">Options used to configure how the sub-type behaves when used by this <see cref="BaseType"/>.</param>
		public void UseSubType(Type subType, Action<PolymorphicOptionsBaseTypeSubType> options)
		{
			var typeInfo = AddSubTypeToThisBaseType(subType);

			options.Invoke(typeInfo);
		}

		private PolymorphicOptionsBaseTypeSubType AddSubTypeToThisBaseType(Type subType)
		{
			if (!knownSubTypes.TryGetValue(subType, out var typeInfo))
			{
				typeInfo = new PolymorphicOptionsBaseTypeSubType(subType);
				knownSubTypes[subType] = typeInfo;
			}

			return typeInfo;
		}
	}
}
