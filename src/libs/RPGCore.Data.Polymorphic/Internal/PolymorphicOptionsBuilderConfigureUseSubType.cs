using System;

namespace RPGCore.Data.Polymorphic.Internal;

internal sealed class PolymorphicOptionsBuilderConfigureUseSubType : IPolymorphicOptionsBuilderConfigure
{
	public Type Type { get; set; }
	public Action<PolymorphicOptionsBuilderExplicitType>? Action { get; set; }

	public PolymorphicOptionsBuilderConfigureUseSubType(Type type)
	{
		Type = type;
	}

	public PolymorphicOptionsBuilderConfigureUseSubType(Type type, Action<PolymorphicOptionsBuilderExplicitType>? action)
	{
		Type = type;
		Action = action;
	}
}
