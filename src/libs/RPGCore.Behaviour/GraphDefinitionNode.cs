using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Behaviour;

public sealed class GraphDefinitionNode
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly Type[] components;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly GraphDefinitionNodeConnectedInput[] connectedInputDefinitions;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly GraphDefinitionNodeOutput[] outputDefinitions;

	/// <summary>
	/// The <see cref="Node"/> that this <see cref="GraphDefinitionNode"/> was created from.
	/// </summary>
	public Node Node { get; }

	/// <summary>
	/// The <see cref="NodeRuntime"/> used to control the behaviour of the <see cref="Node"/>.
	/// </summary>
	public NodeRuntime Runtime { get; }

	/// <summary>
	/// Definitions of all components that the runtime node interface with.
	/// </summary>
	public ReadOnlySpan<Type> Components => components;

	/// <summary>
	/// Definitions of all <b>connected</b> inputs from the node this <see cref="GraphDefinitionNode"/> was created from.
	/// </summary>
	public ReadOnlySpan<GraphDefinitionNodeConnectedInput> ConnectedInputDefinitions => connectedInputDefinitions;

	/// <summary>
	/// Definitions of all outputs from the node this <see cref="GraphDefinitionNode"/> was created from.
	/// </summary>
	public ReadOnlySpan<GraphDefinitionNodeOutput> OutputDefinitions => outputDefinitions;

	internal GraphDefinitionNode(
		Node node,
		NodeRuntime runtime,
		Type[] components,
		GraphDefinitionNodeConnectedInput[] connectedInputDefinitions,
		GraphDefinitionNodeOutput[] outputDefinitions)
	{
		Node = node;
		Runtime = runtime;

		this.components = components;
		this.connectedInputDefinitions = connectedInputDefinitions;
		this.outputDefinitions = outputDefinitions;
	}

	/// <summary>
	/// Begins the construction of a <see cref="GraphDefinitionNode"/> via a <see cref="NodeDefinitionBuilder"/>.
	/// </summary>
	/// <returns>A builder that can be used to extend the <see cref="GraphDefinitionNode"/>.</returns>
	public static NodeDefinitionBuilder Create(Node node)
	{
		return new NodeDefinitionBuilder(node);
	}

	public ref readonly GraphDefinitionNodeConnectedInput GetConnectedInputDefinition(IInput input)
	{
		for (int i = 0; i < outputDefinitions.Length; i++)
		{
			ref var connectedInputDefinition = ref connectedInputDefinitions[i];

			if (connectedInputDefinition.Input == input)
			{
				return ref connectedInputDefinition;
			}
		}
		throw new KeyNotFoundException("Unable to find input in node definition.");
	}

	public ref readonly GraphDefinitionNodeOutput GetOutputDefinition(IOutput output)
	{
		for (int i = 0; i < outputDefinitions.Length; i++)
		{
			ref var outputDefinition = ref outputDefinitions[i];

			if (outputDefinition.Output == output)
			{
				return ref outputDefinition;
			}
		}
		throw new KeyNotFoundException("Unable to find output in node definition.");
	}

	public override string ToString()
	{
		return $"{Node}";
	}
}
