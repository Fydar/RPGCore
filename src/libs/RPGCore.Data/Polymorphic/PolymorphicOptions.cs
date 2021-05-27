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

		internal Dictionary<Type, PolymorphicBaseTypeInfo> knownBaseTypes = new();
		internal Dictionary<Type, PolymorphicSubTypeInfo> knownSubTypes = new();

		public PolymorphicOptions()
		{
		}

		public PolymorphicOptions UseKnownBaseType(Type knownBaseType, Action<PolymorphicBaseTypeInfo> options)
		{
			if (!knownBaseTypes.TryGetValue(knownBaseType, out var typeInfo))
			{
				typeInfo = new PolymorphicBaseTypeInfo(knownBaseType);
				knownBaseTypes.Add(knownBaseType, typeInfo);
			}

			options.Invoke(typeInfo);
			return this;
		}

		public PolymorphicOptions UseKnownSubType(Type knownSubType, Action<PolymorphicSubTypeInfo> options)
		{
			if (!knownSubTypes.TryGetValue(knownSubType, out var typeInfo))
			{
				typeInfo = new PolymorphicSubTypeInfo(knownSubType);
				knownSubTypes.Add(knownSubType, typeInfo);
			}

			options.Invoke(typeInfo);
			return this;
		}
	}
}
