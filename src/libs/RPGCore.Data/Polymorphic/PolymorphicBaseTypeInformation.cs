using System;
using System.Linq;

namespace RPGCore.Data.Polymorphic
{
	public class PolymorphicBaseTypeInformation
	{
		public PolymorphicOptions Options { get; }
		public Type BaseType { get; }
		public PolymorphicSubTypeInformation[] SubTypes { get; }

		public PolymorphicBaseTypeInformation(PolymorphicOptions options, Type baseType)
		{
			Options = options;
			BaseType = baseType;
			SubTypes = PolymorphicSubTypeInformation
				.GetUserDefinedOptions(baseType,
					options.DefaultNamingConvention,
					options.DefaultAliasConventions)
				.ToArray();
		}

		public PolymorphicSubTypeInformation? GetSubTypeInformation(Type valueType)
		{
			PolymorphicSubTypeInformation? typeInformation = null;
			foreach (var polymorphicType in SubTypes)
			{
				if (polymorphicType.Type == valueType)
				{
					typeInformation = polymorphicType;
				}
			}
			return typeInformation;
		}

		public Type? GetTypeForDescriminatorValue(string typeName)
		{
			Type? type = null;
			foreach (var option in SubTypes)
			{
				if (option.DoesNameMatch(typeName, Options.CaseInsensitive))
				{
					type = option.Type;
				}
			}
			return type;
		}
	}
}
