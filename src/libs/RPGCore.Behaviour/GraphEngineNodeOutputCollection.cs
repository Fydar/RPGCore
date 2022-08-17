using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Behaviour;

[DebuggerDisplay("Count = {Count,nq}")]
public readonly ref struct GraphEngineNodeOutputCollection
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly GraphEngine graphEngine;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly int engineNodeIndex;

	/// <summary>
	/// The count of elements contained within this collection.
	/// </summary>
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public int Count => graphEngine.nodes[engineNodeIndex].nodeOutputCount;

	public GraphEngineNodeOutput this[int index]
	{
		get
		{
			return new GraphEngineNodeOutput(graphEngine, graphEngine.nodes[engineNodeIndex].nodeOutputStartIndex + index);
		}
	}

	public GraphEngineNodeOutputCollection(
		GraphEngine graphEngine,
		int engineNodeIndex)
	{
		this.graphEngine = graphEngine;
		this.engineNodeIndex = engineNodeIndex;
	}

	public GraphEngineNodeOutput From(IOutput output)
	{
		ref var node = ref graphEngine.nodes[engineNodeIndex];
		for (int i = 0; i < node.nodeOutputStartIndex; i++)
		{
			var outputIndex = node.nodeOutputStartIndex + i;

			ref var outputData = ref graphEngine.outputs[outputIndex];

			if (outputData.output == output)
			{
				return new GraphEngineNodeOutput(graphEngine, outputIndex);
			}
		}
		throw new KeyNotFoundException($"Unable to find '{output}' in node {engineNodeIndex}.");
	}
}
