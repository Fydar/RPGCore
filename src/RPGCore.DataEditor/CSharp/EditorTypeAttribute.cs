using System;

namespace RPGCore.DataEditor.CSharp
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
	public class EditorTypeAttribute : Attribute
	{
		public EditorTypeAttribute()
		{

		}
	}
}
