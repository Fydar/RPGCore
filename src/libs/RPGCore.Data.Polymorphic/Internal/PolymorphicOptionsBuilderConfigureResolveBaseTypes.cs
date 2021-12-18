using System;

namespace RPGCore.Data.Polymorphic
{
	internal sealed class PolymorphicOptionsBuilderConfigureResolveBaseTypes : IPolymorphicOptionsBuilderConfigure
	{
		public Action<PolymorphicOptionsBuilderResolveBaseType>? Action { get; set; }

		public PolymorphicOptionsBuilderConfigureResolveBaseTypes()
		{
		}

		public PolymorphicOptionsBuilderConfigureResolveBaseTypes(Action<PolymorphicOptionsBuilderResolveBaseType>? action)
		{
			Action = action;
		}
	}
}
