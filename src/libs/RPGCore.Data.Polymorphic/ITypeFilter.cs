using System;

namespace RPGCore.Data.Polymorphic
{
	public interface ITypeFilter
	{
		bool ShouldInclude(Type type);
	}
}
