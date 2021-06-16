using System;
using System.Collections.Generic;

namespace RPGCore.Data.Polymorphic.Configuration
{
	public class PolymorphicConfigurationBaseType
	{
		public PolymorphicConfiguration Configuration { get; }
		public Type BaseType { get; }
		public Dictionary<Type, PolymorphicConfigurationSubType> SubTypes { get; }

		internal PolymorphicConfigurationBaseType(PolymorphicConfiguration configuration, Type baseType)
		{
			Configuration = configuration;
			BaseType = baseType;
			SubTypes = new Dictionary<Type, PolymorphicConfigurationSubType>();
		}

		public PolymorphicConfigurationSubType? GetSubTypeInformation(Type valueType)
		{
			SubTypes.TryGetValue(valueType, out var subType);
			return subType;
		}

		public Type? GetTypeForDescriminatorValue(string typeName)
		{
			Type? type = null;
			foreach (var option in SubTypes.Values)
			{
				if (option.DoesDescriminatorIndicate(typeName, Configuration.CaseInsensitive))
				{
					type = option.Type;
				}
			}
			return type;
		}

		public override string ToString()
		{
			return BaseType.Name;
		}
	}
}
