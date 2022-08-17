using System;

namespace RPGCore.Behaviour;

public sealed class GraphInstanceData
{
	public GraphInstanceDataNode[] Nodes { get; set; } = Array.Empty<GraphInstanceDataNode>();

	public ref GraphInstanceDataNode GetNodeData(string id)
	{
		for (int i = 0; i < Nodes.Length; i++)
		{
			ref var node = ref Nodes[i];
			if (node.Id == id)
			{
				return ref node;
			}
		}
		throw new InvalidOperationException($"Unable to find node runtime data with id {id}.");
	}

	public bool ContainsNode(string id)
	{
		for (int i = 0; i < Nodes.Length; i++)
		{
			ref var node = ref Nodes[i];
			if (node.Id == id)
			{
				return true;
			}
		}
		return false;
	}
}
