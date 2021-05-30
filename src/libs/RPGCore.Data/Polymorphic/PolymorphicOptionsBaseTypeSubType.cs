using System;
using System.Collections.Generic;

namespace RPGCore.Data.Polymorphic
{
	public class PolymorphicOptionsBaseTypeSubType
	{
		/// <summary>
		/// The type that this <see cref="PolymorphicOptionsBaseTypeSubType"/> represents.
		/// </summary>
		public Type SubType { get; }

		/// <summary>
		/// The primary alias for this subtype that is used to identify this sub-type.
		/// </summary>
		public string? Descriminator { get; set; }

		/// <summary>
		/// Aliases for this subtype that as are also acceptable to indicate this sub-type.
		/// </summary>
		public List<string> Aliases { get; set; }

		public PolymorphicOptionsBaseTypeSubType(Type subType)
		{
			SubType = subType;
			Aliases = new List<string>();
		}
	}
}
