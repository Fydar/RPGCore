using System.Collections.Generic;

namespace RPGCore.Behaviour;

public sealed class NodeDefinition
{
	/// <summary>
	/// The <see cref="Node"/> that this <see cref="NodeDefinition"/> was created from.
	/// </summary>
	public Node Node { get; }

	/// <summary>
	/// The <see cref="NodeRuntime"/> used to control the behaviour of the <see cref="Node"/>.
	/// </summary>
	public NodeRuntime Runtime { get; }

	/// <summary>
	/// Definitions of all inputs from the node this <see cref="NodeDefinition"/> was created from.
	/// </summary>
	public IReadOnlyList<NodeInputDefinition> Inputs { get; }

	/// <summary>
	/// Definitions of all outputs from the node this <see cref="NodeDefinition"/> was created from.
	/// </summary>
	public IReadOnlyList<NodeOutputDefinition> Outputs { get; }

	internal NodeDefinition(
		Node node,
		NodeRuntime runtime,
		IReadOnlyList<NodeInputDefinition> inputs,
		IReadOnlyList<NodeOutputDefinition> outputs)
	{
		Node = node;
		Runtime = runtime;
		Inputs = inputs;
		Outputs = outputs;
	}

	/// <summary>
	/// Begins the construction of a <see cref="NodeDefinition"/> via a <see cref="NodeDefinitionBuilder"/>.
	/// </summary>
	/// <returns>A builder that can be used to extend the <see cref="NodeDefinition"/>.</returns>
	public static NodeDefinitionBuilder Create(Node node)
	{
		return new NodeDefinitionBuilder(node);
	}
}
