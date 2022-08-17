﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Behaviour;

public sealed class NodeDefinition
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	internal readonly Type[] components;
	
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly NodeDefinitionInput[] inputDefinitions;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly NodeDefinitionOutput[] outputDefinitions;

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
	public ReadOnlySpan<NodeDefinitionInput> InputDefinitions => inputDefinitions;

	/// <summary>
	/// Definitions of all outputs from the node this <see cref="GraphDefinitionNode"/> was created from.
	/// </summary>
	public ReadOnlySpan<NodeDefinitionOutput> OutputDefinitions => outputDefinitions;

	internal NodeDefinition(
		Node node,
		NodeRuntime runtime,
		Type[] components,
		NodeDefinitionInput[] inputDefinitions,
		NodeDefinitionOutput[] outputDefinitions)
	{
		Node = node;
		Runtime = runtime;

		this.components = components;
		this.inputDefinitions = inputDefinitions;
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

	public ref readonly NodeDefinitionInput GetInputDefinition(IInput input)
	{
		for (int i = 0; i < inputDefinitions.Length; i++)
		{
			ref var inputDefinition = ref inputDefinitions[i];

			if (inputDefinition.Input == input)
			{
				return ref inputDefinition;
			}
		}
		throw new KeyNotFoundException("Unable to find input in node definition.");
	}

	public ref readonly NodeDefinitionOutput GetOutputDefinition(IOutput output)
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
}
