using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Behaviour;

[DebuggerDisplay("Count = {Count,nq}")]
public readonly ref struct GraphEngineNodeConnectedInputCollection
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly GraphEngine graphEngine;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly int engineNodeIndex;

	/// <summary>
	/// The count of elements contained within this collection.
	/// </summary>
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public int Count => graphEngine.nodes[engineNodeIndex].nodeConnectedInputCount;

	public GraphEngineNodeConnectedInput this[int index]
	{
		get
		{
			return new GraphEngineNodeConnectedInput(graphEngine, graphEngine.nodes[engineNodeIndex].nodeConnectedInputStartIndex + index);
		}
	}

	public GraphEngineNodeConnectedInputCollection(
		GraphEngine graphEngine,
		int engineNodeIndex)
	{
		this.graphEngine = graphEngine;
		this.engineNodeIndex = engineNodeIndex;
	}

	public GraphEngineNodeConnectedInput From(IInput input)
	{
		ref var node = ref graphEngine.nodes[engineNodeIndex];
		for (int i = 0; i < node.nodeConnectedInputCount; i++)
		{
			int connectedInputIndex = node.nodeConnectedInputStartIndex + i;

			ref var connectedInputData = ref graphEngine.connectedInputs[connectedInputIndex];

			if (connectedInputData.input == input)
			{
				return new GraphEngineNodeConnectedInput(graphEngine, connectedInputIndex);
			}
		}
		throw new KeyNotFoundException($"Unable to find '{input}' in node {engineNodeIndex}.");
	}
}
