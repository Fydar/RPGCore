using System;

namespace RPGCore.Data.Polymorphic
{
	internal sealed class PolymorphicOptionsBuilderConfigureUseBaseType : IPolymorphicOptionsBuilderConfigure
	{
		public Type Type { get; set; }
		public Action<PolymorphicOptionsBuilderExplicitType>? Action { get; set; }

		public PolymorphicOptionsBuilderConfigureUseBaseType(Type type)
		{
			Type = type;
		}

		public PolymorphicOptionsBuilderConfigureUseBaseType(Type type, Action<PolymorphicOptionsBuilderExplicitType>? action) : this(type)
		{
			Action = action;
		}
	}
}
