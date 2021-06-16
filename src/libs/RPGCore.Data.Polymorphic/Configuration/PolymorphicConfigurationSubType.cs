using System;

namespace RPGCore.Data.Polymorphic.Configuration
{
	public class PolymorphicConfigurationSubType
	{
		public PolymorphicConfiguration Configuration { get; }
		public Type Type { get; }
		public PolymorphicConfigurationBaseType BaseType { get; }
		public string Name { get; set; }
		public string[] Aliases { get; set; }

		public PolymorphicConfigurationSubType(PolymorphicConfiguration configuration, Type type, PolymorphicConfigurationBaseType baseType)
		{
			Configuration = configuration;
			Type = type;
			Name = type.FullName;
			Aliases = Array.Empty<string>();
			BaseType = baseType;
		}

		public bool DoesDescriminatorIndicate(string descriminator, bool caseInsentitive)
		{
			var comparison = caseInsentitive
				? StringComparison.OrdinalIgnoreCase
				: StringComparison.Ordinal;

			if (string.Equals(Name, descriminator, comparison))
			{
				return true;
			}

			foreach (string alias in Aliases)
			{
				if (string.Equals(alias, descriminator, comparison))
				{
					return true;
				}
			}
			return false;
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
