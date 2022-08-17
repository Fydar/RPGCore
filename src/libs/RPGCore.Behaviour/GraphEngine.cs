using RPGCore.Behaviour.Internal;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Behaviour;

public sealed class GraphEngine
{
	// [DebuggerBrowsable(DebuggerBrowsableState.Never)]
	internal readonly GraphEngineNodeData[] nodes;

	// [DebuggerBrowsable(DebuggerBrowsableState.Never)]
	internal readonly GraphEngineNodeComponentData[] components;

	// [DebuggerBrowsable(DebuggerBrowsableState.Never)]
	internal readonly GraphEngineNodeConnectedInputData[] connectedInputs;

	// [DebuggerBrowsable(DebuggerBrowsableState.Never)]
	internal readonly GraphEngineNodeOutputConnectedInputData[] outputConnectedInputs;

	// [DebuggerBrowsable(DebuggerBrowsableState.Never)]
	internal readonly GraphEngineNodeOutputData[] outputs;

	public GraphEngineNodeCollection Nodes => new(this);

	internal GraphEngine(
		GraphDefinition graphDefinition)
	{
		nodes = new GraphEngineNodeData[graphDefinition.GraphDefinitionNode.Count];

		var componentsData = new List<GraphEngineNodeComponentData>();
		var connectedInputsData = new List<GraphEngineNodeConnectedInputData>();
		var outputConnectedInputsData = new List<GraphEngineNodeOutputConnectedInputData>();
		var outputsData = new List<GraphEngineNodeOutputData>();

		for (int i = 0; i < graphDefinition.GraphDefinitionNode.Count; i++)
		{
			var nodeDefinition = graphDefinition.GraphDefinitionNode[i];
			ref var graphEngineNodeData = ref nodes[i];

			graphEngineNodeData.node = nodeDefinition.Node;
			graphEngineNodeData.nodeRuntime = nodeDefinition.Runtime;

			// Collect the components associated with the node.
			graphEngineNodeData.componentsCount = nodeDefinition.Components.Length;
			if (graphEngineNodeData.componentsCount > 0)
			{
				graphEngineNodeData.componentsStartIndex = componentsData.Count;
			}

			for (int j = 0; j < nodeDefinition.Components.Length; j++)
			{
				var componentDefinition = nodeDefinition.Components[j];
				componentsData.Add(new GraphEngineNodeComponentData(componentDefinition));
			}

			// Collected the connected input definitions associated with the node.
			graphEngineNodeData.nodeConnectedInputCount = nodeDefinition.ConnectedInputDefinitions.Length;
			if (graphEngineNodeData.nodeConnectedInputCount > 0)
			{
				graphEngineNodeData.nodeConnectedInputStartIndex = connectedInputsData.Count;
			}

			for (int j = 0; j < nodeDefinition.ConnectedInputDefinitions.Length; j++)
			{
				var connectedInputDefinition = nodeDefinition.ConnectedInputDefinitions[j];

				var connectedInputDefinitionSource = graphDefinition.GraphDefinitionNode[connectedInputDefinition.ConnectedToNode].OutputDefinitions[connectedInputDefinition.ConnectedToNodeOutput];

				connectedInputsData.Add(new GraphEngineNodeConnectedInputData(connectedInputDefinition.Input, connectedInputDefinitionSource.LocalId));

			}
		}

		for (int i = 0; i < graphDefinition.GraphDefinitionNode.Count; i++)
		{
			var nodeDefinition = graphDefinition.GraphDefinitionNode[i];

			for (int j = 0; j < nodeDefinition.OutputDefinitions.Length; j++)
			{
				var outputDefinition = nodeDefinition.OutputDefinitions[j];

				var outputData = new GraphEngineNodeOutputData(outputDefinition.Output, outputDefinition.Name, 0, 0);

				outputData.outputConnectedInputsCount = outputDefinition.ConnectedInputIndexes.Length;
				if (outputData.outputConnectedInputsCount > 0)
				{
					outputData.outputConnectedInputsStartIndex = outputConnectedInputsData.Count;
				}

				for (int k = 0; k < outputDefinition.ConnectedInputIndexes.Length; k++)
				{
					var connectedInputIndex = outputDefinition.ConnectedInputIndexes[k];


				}

				outputsData.Add(outputData);
			}
		}

		for (int i = 0; i < outputsData.Count; i++)
		{
			var outputData = outputsData[i];


		}

		for (int i = 0; i < graphDefinition.GraphDefinitionNode.Count; i++)
		{
			var nodeDefinition = graphDefinition.GraphDefinitionNode[i];

			for (int j = 0; j < nodeDefinition.OutputDefinitions.Length; j++)
			{
				var outputDefinition = nodeDefinition.OutputDefinitions[j];

				outputsData.Add(new GraphEngineNodeOutputData(outputDefinition.Output, outputDefinition.Name, 0, 0));
			}
		}

		components = componentsData.ToArray();
		connectedInputs = connectedInputsData.ToArray();
		outputConnectedInputs = outputConnectedInputsData.ToArray();
		outputs = outputsData.ToArray();
	}

	public GraphEngineNode GetNode(Node node)
	{
		for (int i = 0; i < nodes.Length; i++)
		{
			var graphEngineNodeData = nodes[i];
			if (graphEngineNodeData.node == node)
			{
				return new GraphEngineNode(this, i);
			}
		}
		throw new KeyNotFoundException($"Unable to find {node} in {nameof(GraphEngine)}.");
	}

	public GraphInstanceData CreateInstanceData()
	{
		return new GraphInstanceData();
	}

	public GraphInstance CreateInstance()
	{
		return new GraphInstance();
	}
}
