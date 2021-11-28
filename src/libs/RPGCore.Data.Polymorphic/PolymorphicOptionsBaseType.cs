using System;
using System.Collections.Generic;

namespace RPGCore.Data.Polymorphic
{
	/// <summary>
	/// Options for configuring the polymorphic types available to the serializer.
	/// </summary>
	public class PolymorphicOptionsBaseType
	{
		/// <summary>
		/// A collection of sub-types that are explicitly declared as suitable for this base-type.
		/// </summary>
		internal readonly Dictionary<Type, PolymorphicOptionsBaseTypeSubType> knownSubTypes;

		internal readonly List<PolymorphicOptionsResolveSubType> resolveSubTypeOptions;

		/// <summary>
		/// The type that this <see cref="PolymorphicOptionsBaseType"/> represents.
		/// </summary>
		public Type BaseType { get; }

		internal PolymorphicOptionsBaseType(Type baseType)
		{
			BaseType = baseType;
			knownSubTypes = new Dictionary<Type, PolymorphicOptionsBaseTypeSubType>();
			resolveSubTypeOptions = new List<PolymorphicOptionsResolveSubType>();
		}

		/// <summary>
		/// Directs the serializer to locate additional sub-types automatically.
		/// </summary>
		public void ResolveSubTypesAutomatically()
		{
			resolveSubTypeOptions.Add(new PolymorphicOptionsResolveSubType());
		}

		/// <summary>
		/// Directs the serializer to locate additional sub-types automatically.
		/// </summary>
		public void ResolveSubTypesAutomatically(Action<PolymorphicOptionsResolveSubType> options)
		{
			var optionsResult = new PolymorphicOptionsResolveSubType();

			options?.Invoke(optionsResult);

			resolveSubTypeOptions.Add(optionsResult);
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

		/// <summary>
		/// Adds a sub-type to a <see cref="BaseType"/> list of valid sub-types.
		/// </summary>
		/// <typeparam name="TSubType">The sub-type to allow for this <see cref="BaseType"/>.</typeparam>
		/// <param name="options">Options used to configure how the sub-type behaves when used by this <see cref="BaseType"/>.</param>
		public void UseSubType<TSubType>(Action<PolymorphicOptionsBaseTypeSubType> options)
		{
			var typeInfo = AddSubTypeToThisBaseType(typeof(TSubType));
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
