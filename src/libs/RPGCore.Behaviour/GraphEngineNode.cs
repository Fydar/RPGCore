using System;
using System.Diagnostics;

namespace RPGCore.Behaviour;

public readonly ref struct GraphEngineNode
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly GraphEngine graphEngine;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly int engineNodeIndex;

	public GraphEngineNodeComponentCollection Components => new(graphEngine, engineNodeIndex);

	public GraphEngineNodeOutputCollection Outputs => new(graphEngine, engineNodeIndex);

	public GraphEngineNodeConnectedInputCollection ConnectedInputs => new(graphEngine, engineNodeIndex);

	public Node Node => graphEngine.nodes[engineNodeIndex].node;

	public NodeRuntime Runtime => graphEngine.nodes[engineNodeIndex].nodeRuntime;

	public GraphEngineNode(
		GraphEngine graphEngine,
		int engineNodeIndex)
	{
		this.graphEngine = graphEngine;
		this.engineNodeIndex = engineNodeIndex;
	}
}
