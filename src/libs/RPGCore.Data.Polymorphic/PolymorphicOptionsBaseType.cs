using System;
using System.Collections.Generic;

namespace RPGCore.Data.Polymorphic
{
	public class PolymorphicOptionsBaseType
	{
		internal PolymorphicOptionsResolveSubType? resolveSubTypeOptions;

		/// <summary>
		/// The type that this <see cref="PolymorphicOptionsBaseType"/> represents.
		/// </summary>
		public Type BaseType { get; }

		/// <summary>
		/// A collection of sub-types that are explicitly declared as suitable for this base type.
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

		public void UseSubType(Type subType)
		{
			if (!knownSubTypes.ContainsKey(subType))
			{
				var typeInfo = new PolymorphicOptionsBaseTypeSubType(subType);
				knownSubTypes[subType] = typeInfo;
			}
		}

		public void UseSubType(Type subType, Action<PolymorphicOptionsBaseTypeSubType> options)
		{
			if (!knownSubTypes.TryGetValue(subType, out var typeInfo))
			{
				typeInfo = new PolymorphicOptionsBaseTypeSubType(subType);
				knownSubTypes[subType] = typeInfo;
			}

			options.Invoke(typeInfo);
		}
	}
}
