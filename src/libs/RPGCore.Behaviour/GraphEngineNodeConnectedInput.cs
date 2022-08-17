using System.Diagnostics;

namespace RPGCore.Behaviour;

public readonly ref struct GraphEngineNodeConnectedInput
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly GraphEngine graphEngine;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly int connectedInputIndex;

	public GraphEngineNodeConnectedInput(
		GraphEngine graphEngine,
		int connectedInputIndex)
	{
		this.graphEngine = graphEngine;
		this.connectedInputIndex = connectedInputIndex;
	}
}
