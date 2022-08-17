using System;

namespace RPGCore.Behaviour.Internal;

internal readonly struct GraphEngineNodeComponentData
{
	internal readonly Type type;

	public GraphEngineNodeComponentData(
		Type type)
	{
		this.type = type;
	}
}
