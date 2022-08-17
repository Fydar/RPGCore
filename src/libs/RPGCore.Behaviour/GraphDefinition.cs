using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Behaviour;

public sealed class GraphDefinition
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly GraphDefinitionNode[] graphDefinitionNode;

	/// <summary>
	/// The <see cref="Graph"/> that this <see cref="GraphDefinition"/> was generated from.
	/// </summary>
	public Graph Graph { get; }

	public IReadOnlyList<GraphDefinitionNode> GraphDefinitionNode => graphDefinitionNode;

	internal GraphDefinition(Graph graph)
	{
		Graph = graph;

		graphDefinitionNode = new GraphDefinitionNode[graph.Nodes.Length];

		var nodeDefinitions = new NodeDefinition[graph.Nodes.Length];
		for (int i = 0; i < graph.Nodes.Length; i++)
		{
			var node = graph.Nodes[i];
			nodeDefinitions[i] = node.CreateDefinition();
		}

		var nodeConnectedInputs = new List<GraphDefinitionNodeConnectedInput>[nodeDefinitions.Length];

		for (int i = 0; i < nodeDefinitions.Length; i++)
		{
			var nodeDefinition = nodeDefinitions[i];

			var connectedInputDefinition = new List<GraphDefinitionNodeConnectedInput>();
			foreach (var inputDefinition in nodeDefinition.InputDefinitions)
			{
				if (inputDefinition.Input is ConnectedInput connectedInput)
				{
					for (int k = 0; k < nodeDefinitions.Length; k++)
					{
						var searchNode = nodeDefinitions[k];
						for (int l = 0; l < searchNode.OutputDefinitions.Length; l++)
						{
							var searchNodeOutputDefinition = searchNode.OutputDefinitions[l];

							if (searchNodeOutputDefinition.Name == connectedInput.Path)
							{
								connectedInputDefinition.Add(new GraphDefinitionNodeConnectedInput()
								{
									Input = connectedInput,
									ConnectedToNode = k,
									ConnectedToNodeOutput = l,
								});
								break;
							}
						}
					}
				}
			}

			nodeConnectedInputs[i] = connectedInputDefinition;
		}

		int outputLocalIdCounter = 0;

		for (int i = 0; i < nodeDefinitions.Length; i++)
		{
			var nodeDefinition = nodeDefinitions[i];

			var outputDefinitions = new GraphDefinitionNodeOutput[nodeDefinition.OutputDefinitions.Length];

			for (int j = 0; j < nodeDefinition.OutputDefinitions.Length; j++)
			{
				var outputDefinition = nodeDefinition.OutputDefinitions[j];

				var connectedInputs = new List<GraphDefinitionNodeOutputConnectedInput>();

				for (int k = 0; k < nodeConnectedInputs.Length; k++)
				{
					var searchNodeConnectedInputs = nodeConnectedInputs[k];

					if (searchNodeConnectedInputs != null)
					{
						for (int l = 0; l < searchNodeConnectedInputs.Count; l++)
						{
							var searchNodeConnectedInput = searchNodeConnectedInputs[l];
							if (searchNodeConnectedInput.ConnectedToNode == i
								&& searchNodeConnectedInput.ConnectedToNodeOutput == l)
							{
								connectedInputs.Add(new GraphDefinitionNodeOutputConnectedInput(k, l));
							}
						}
					}
				}

				outputDefinitions[j] = new GraphDefinitionNodeOutput(outputDefinition.Output, outputLocalIdCounter, outputDefinition.Name)
				{
					ConnectedInputIndexes = connectedInputs.ToArray()
				};
				outputLocalIdCounter++;
			}

			graphDefinitionNode[i] = new GraphDefinitionNode(
				nodeDefinition.Node,
				nodeDefinition.Runtime,
				nodeDefinition.components,
				nodeConnectedInputs[i].ToArray(),
				outputDefinitions);
		}
	}

	public GraphEngine CreateEngine()
	{
		return new GraphEngine(this);
	}

	public GraphDefinitionNode? GetNodeDefinition(Node node)
	{
		foreach (var nodeDefinition in GraphDefinitionNode)
		{
			if (nodeDefinition.Node == node)
			{
				return nodeDefinition;
			}
		}
		return null;
	}
}
