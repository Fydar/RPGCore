using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Behaviour;

[DebuggerDisplay("Count = {Count,nq}")]
public readonly ref struct GraphEngineNodeOutputConnectedInputsCollection
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly GraphEngine graphEngine;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly int engineNodeOutputIndex;

	/// <summary>
	/// The count of elements contained within this collection.
	/// </summary>
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public int Count => graphEngine.outputs[engineNodeOutputIndex].outputConnectedInputsCount;

	public GraphEngineNodeConnectedInput this[int index]
	{
		get
		{
			return new GraphEngineNodeConnectedInput(graphEngine, graphEngine.outputConnectedInputs[graphEngine.outputs[engineNodeOutputIndex].outputConnectedInputsStartIndex + index].connectedInputIndex);
		}
	}

	public GraphEngineNodeOutputConnectedInputsCollection(
		GraphEngine graphEngine,
		int engineNodeOutputIndex)
	{
		this.graphEngine = graphEngine;
		this.engineNodeOutputIndex = engineNodeOutputIndex;
	}

	public GraphEngineNodeConnectedInput From(IInput input)
	{
		ref var output = ref graphEngine.outputs[engineNodeOutputIndex];
		for (int i = 0; i < output.outputConnectedInputsCount; i++)
		{
			int outputConnectedInputIndex = output.outputConnectedInputsStartIndex + i;

			ref var outputConnectedInputData = ref graphEngine.outputConnectedInputs[outputConnectedInputIndex];
			ref var connectedInputData = ref graphEngine.connectedInputs[outputConnectedInputData.connectedInputIndex];
			if (connectedInputData.input == input)
			{
				return new GraphEngineNodeConnectedInput(graphEngine, outputConnectedInputData.connectedInputIndex);
			}
		}
		throw new KeyNotFoundException($"Unable to find '{input}' in outputs of a node.");
	}
}
