using System;
using System.Collections.Generic;

namespace RPGCore.Behaviour;

public struct GraphInstanceNode
{
	public GraphInstance GraphRuntime { get; }
	public Node Node { get; }

	public GraphInstanceNode(
		GraphInstance graphRuntime,
		Node node)
	{
		GraphRuntime = graphRuntime;
		Node = node;
	}
}

public struct GraphInstanceNode<TNode>
	where TNode : Node
{
	public GraphInstance GraphRuntime { get; }
	public TNode Node { get; }

	public GraphInstanceNode(
		GraphInstance graphRuntime,
		TNode node)
	{
		GraphRuntime = graphRuntime;
		Node = node;
	}

	public void OpenInput<TType>(
		IInput<TType> input,
		out GraphInstanceInput<TType> runtimeInput)
	{
		if (input is ConnectedInput<TType> connectedInput)
		{
			string outputIdentifier = connectedInput.Path.ToString();
			var outputPath = new LocalPropertyId(outputIdentifier);

			var graphEngineNode = GraphRuntime.GraphEngine.GetNode(Node);

			var graphEngineNodeInput = graphEngineNode.ConnectedInputs.From(input);

			// GraphRuntime.GraphDefinition.

			ref var nodeData = ref GraphRuntime.GraphRuntimeData.GetNodeData(outputPath.TargetIdentifier.ToString());

			if (nodeData.Outputs == null)
			{
				nodeData.Outputs = new Dictionary<string, IOutputData>();
			}

			if (nodeData.Outputs.TryGetValue(outputPath.PropertyPath[0], out var outputData))
			{
				var castedOutputData = (Output<TType>.OutputData)outputData;
				runtimeInput = new GraphInstanceInput<TType>(GraphRuntime.GraphRuntimeData, input, castedOutputData.Value, false);
			}
			else
			{
				throw new InvalidOperationException("Failed to locate Output that the input is connected to.");
			}
		}
		else if (input is DefaultInput<TType> defaultInput)
		{
			runtimeInput = new GraphInstanceInput<TType>(GraphRuntime.GraphRuntimeData, input, defaultInput.DefaultValue, false);
		}
		else
		{
			throw new InvalidOperationException();
		}
	}

	public void OpenOutput<TType>(Output<TType> output, out GraphInstanceOutput<TType> runtimeOutput)
	{
		var graphEngineNode = GraphRuntime.GraphEngine.GetNode(Node);

		var graphEngineNodeOutput = graphEngineNode.Outputs.From(output);

		ref var nodeData = ref GraphRuntime.GraphRuntimeData.GetNodeData(Node.Id);

		runtimeOutput = nodeData.GetOrCreateOutput<TType>(graphEngineNodeOutput);
	}

	public ref TComponent GetComponent<TComponent>()
		where TComponent : struct, INodeComponent
	{
		ref var nodeData = ref GraphRuntime.GraphRuntimeData.GetNodeData(Node.Id);

		return ref nodeData.GetComponent<TComponent>();
	}
}
