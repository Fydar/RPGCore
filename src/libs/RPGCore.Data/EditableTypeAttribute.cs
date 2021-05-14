using System;

namespace RPGCore.DataEditor.Manifest.Source.RuntimeSource
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
	public class EditableTypeAttribute : Attribute
	{
		public EditableTypeAttribute()
		{

		}
	}
}
