using RPGCore.Data.Polymorphic.Naming;
using System.Collections.Generic;

namespace RPGCore.Data.Polymorphic
{
	public class PolymorphicOptionsResolveSubType
	{
		public ITypeFilter? TypeFilter { get; set; }

		public ITypeNamingConvention TypeNaming { get; set; }

		public List<ITypeNamingConvention> TypeAliasing { get; }

		internal PolymorphicOptionsResolveSubType()
		{
			TypeAliasing = new List<ITypeNamingConvention>();
		}
	}
}
