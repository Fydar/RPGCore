using System;
using System.Diagnostics;

namespace RPGCore.Behaviour;

public readonly ref struct GraphEngineNodeComponent
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly GraphEngine graphEngine;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly int engineNodeComponentIndex;

	public Type ComponentType => graphEngine.components[engineNodeComponentIndex].type;

	public GraphEngineNodeComponent(
		GraphEngine graphEngine,
		int engineNodeComponentIndex)
	{
		this.graphEngine = graphEngine;
		this.engineNodeComponentIndex = engineNodeComponentIndex;
	}
}
