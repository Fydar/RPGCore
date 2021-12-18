using RPGCore.Data.Polymorphic.Naming;
using System.Collections.Generic;

namespace RPGCore.Data.Polymorphic
{
	/// <summary>
	/// Options for how sub-types should be automatically resolved.
	/// </summary>
	public class PolymorphicOptionsBuilderResolveSubType
	{
		/// <summary>
		/// A type filter used to limit the scope of resolved sub-types.
		/// </summary>
		public ITypeFilter? TypeFilter { get; set; }

		/// <summary>
		/// A <see cref="ITypeNamingConvention"/> used to determine the identifier for the resolved sub-types.
		/// </summary>
		/// <remarks>
		/// If no value for this property is assigned then <see cref="PolymorphicOptionsBuilder.DefaultNamingConvention"/> is used.
		/// </remarks>
		public ITypeNamingConvention? TypeNaming { get; set; }

		/// <summary>
		/// A collection of <see cref="ITypeNamingConvention"/> used to determine aliases for the resolved sub-types.
		/// </summary>
		public List<ITypeNamingConvention> TypeAliasing { get; }

		internal PolymorphicOptionsBuilderResolveSubType()
		{
			TypeAliasing = new List<ITypeNamingConvention>();
		}
	}
}
