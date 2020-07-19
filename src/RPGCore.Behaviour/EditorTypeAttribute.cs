using System;

namespace RPGCore.Behaviour
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
	public class EditorTypeAttribute : Attribute
	{
		public EditorTypeAttribute()
		{

		}
	}
}
