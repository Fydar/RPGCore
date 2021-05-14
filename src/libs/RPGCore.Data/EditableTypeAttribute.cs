using System;

namespace RPGCore.Data
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
	public class EditableTypeAttribute : Attribute
	{
		public EditableTypeAttribute()
		{

		}
	}
}
