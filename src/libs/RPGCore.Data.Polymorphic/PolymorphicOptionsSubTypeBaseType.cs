using System;
using System.Collections.Generic;

namespace RPGCore.Data.Polymorphic
{
	public class PolymorphicOptionsSubTypeBaseType
	{
		public Type BaseType { get; }

		/// <summary>
		/// The primary alias for this subtype that is used to identify this sub-type.
		/// </summary>
		public string? Descriminator { get; set; }

		/// <summary>
		/// Aliases for this subtype that as are also acceptable to indicate this sub-type.
		/// </summary>
		public List<string> Aliases { get; set; }

		internal PolymorphicOptionsSubTypeBaseType(Type baseType)
		{
			BaseType = baseType;
			Aliases = new List<string>();
		}
	}
}
