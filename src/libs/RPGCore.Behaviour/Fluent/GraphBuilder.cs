using System;
using System.Collections.Generic;

namespace RPGCore.Behaviour.Fluent;

public class GraphBuilder
{
	private readonly List<Node> nodes = new();

	public GraphBuilder AddNode<TNode>(Action<TNode> node)
		where TNode : Node, new()
	{
		var newNode = new TNode
		{
			Id = LocalId.NewShortId().ToString()
		};
		node.Invoke(newNode);
		nodes.Add(newNode);
		return this;
	}

	public GraphBuilder AddNode<TNode>(Action<TNode> node, out TNode output)
		where TNode : Node, new()
	{
		output = new TNode
		{
			Id = LocalId.NewShortId().ToString()
		};
		node.Invoke(output);
		nodes.Add(output);
		return this;
	}

	public Graph Build()
	{
		return new Graph(nodes.ToArray());
	}
}
