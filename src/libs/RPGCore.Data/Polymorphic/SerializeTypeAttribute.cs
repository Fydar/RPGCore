using System;

namespace RPGCore.Data.Polymorphic
{
	[AttributeUsage(
		AttributeTargets.Class |
		AttributeTargets.Field |
		AttributeTargets.Property |
		AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
	public class SerializeTypeAttribute : Attribute
	{
		public Type[] Types { get; }

		public SerializeTypeAttribute(params Type[] types)
		{
			Types = types;
		}
	}
}
