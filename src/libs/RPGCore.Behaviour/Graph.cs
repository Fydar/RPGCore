using System;

namespace RPGCore.Behaviour;

/// <summary>
/// A graph that defines a data-driven behaviour.
/// </summary>
public sealed class Graph
{
	public Node[] Nodes { get; set; }

	/// <summary>
	/// Creates a new instance of the <see cref="Graph"/> class.
	/// </summary>
	public Graph()
	{
		Nodes = Array.Empty<Node>();
	}

	/// <summary>
	/// Creates a new instance of the <see cref="Graph"/> class.
	/// </summary>
	/// <param name="nodes">A collection of nodes in the <see cref="Graph"/>.</param>
	public Graph(
		Node[] nodes)
	{
		Nodes = nodes;
	}

	/// <summary>
	/// Creates a <see cref="GraphDefinition"/> for this <see cref="Graph"/>.
	/// </summary>
	/// <returns>A <see cref="GraphDefinition"/> for this <see cref="Graph"/>.</returns>
	public GraphDefinition CreateDefinition()
	{
		return new GraphDefinition(this);
	}
}
