using System;

namespace RPGCore.Data.Polymorphic.Naming
{
	public interface ITypeNamingConvention
	{
		string GetNameForType(Type type);
	}
}
