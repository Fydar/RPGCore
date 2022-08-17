using RPGCore.Behaviour.Fluent;
using System;

namespace RPGCore.Behaviour;

/// <summary>
/// A graph that defines a data-driven behaviour.
/// </summary>
public sealed class Graph
{
	/// <summary>
	/// A collection of <see cref="Node"/> that make up this <see cref="Graph"/>.
	/// </summary>
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

	/// <summary>
	/// Constructs a new instance of a <see cref="Graph"/> via a <see cref="GraphBuilder"/>.
	/// </summary>
	/// <returns>A <see cref="GraphBuilder"/> for use in constructing a <see cref="Graph"/>.</returns>
	public static GraphBuilder Create()
	{
		return new GraphBuilder();
	}
}
