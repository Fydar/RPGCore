using System;

namespace RPGCore.Data.Polymorphic
{
	[Flags]
	public enum TypeName
	{
		None = 0,
		FullName = 1,
		Name = 2,
		AssemblyQualifiedName = 4,
		GUID = 8
	}
}
