using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Behaviour;

public sealed class GraphDefinition
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly NodeDefinition[] nodeDefinitions;

	/// <summary>
	/// The <see cref="Graph"/> that this <see cref="GraphDefinition"/> was generated from.
	/// </summary>
	public Graph Graph { get; }

	public IReadOnlyList<NodeDefinition> NodeDefinitions => nodeDefinitions;

	internal GraphDefinition(Graph graph)
	{
		nodeDefinitions = new NodeDefinition[graph.Nodes.Length];
		for (int i = 0; i < graph.Nodes.Length; i++)
		{
			var node = graph.Nodes[i];

			nodeDefinitions[i] = node.CreateDefinition();
		}
		Graph = graph;
	}

	public GraphRuntimeData CreateRuntimeData()
	{
		return new GraphRuntimeData();
	}

	public GraphRuntime CreateRuntime()
	{
		return new GraphRuntime();
	}
}
