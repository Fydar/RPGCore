using System;
using System.Collections.Generic;

namespace RPGCore.Data.Polymorphic
{
	public class PolymorphicSubTypeInfo
	{
		public Type SubType { get; }
		public string? Descriminator { get; set; }
		public List<string> Aliases { get; set; }

		public PolymorphicSubTypeInfo(Type subType)
		{
			SubType = subType;
			Aliases = new List<string>();
		}
	}
}
