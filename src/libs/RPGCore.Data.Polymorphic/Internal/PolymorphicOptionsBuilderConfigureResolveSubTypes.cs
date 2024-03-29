﻿using System;

namespace RPGCore.Data.Polymorphic.Internal;

internal sealed class PolymorphicOptionsBuilderConfigureResolveSubTypes : IPolymorphicOptionsBuilderConfigure
{
	public Action<PolymorphicOptionsBuilderResolveSubType>? Action { get; set; }

	public PolymorphicOptionsBuilderConfigureResolveSubTypes()
	{
	}

	public PolymorphicOptionsBuilderConfigureResolveSubTypes(Action<PolymorphicOptionsBuilderResolveSubType>? action)
	{
		Action = action;
	}
}
