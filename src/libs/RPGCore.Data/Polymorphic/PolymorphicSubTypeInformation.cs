using System;

namespace RPGCore.Data.Polymorphic
{
	public class PolymorphicSubTypeInformation
	{
		public Type Type { get; set; }
		public string Name { get; set; }
		public string[] Aliases { get; set; }

		public PolymorphicSubTypeInformation(Type type)
		{
			Type = type;
			Name = type.FullName;
			Aliases = Array.Empty<string>();
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
	}
}
