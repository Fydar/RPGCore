using RPGCore.Behaviour.Internal;
using System;
using System.Collections.Generic;

namespace RPGCore.Behaviour;

public sealed class NodeDefinitionBuilder
{
	private readonly Node node;
	private readonly List<NodeInputDefinition> inputs = new();
	private readonly List<NodeOutputDefinition> outputs = new();
	private NodeRuntime? runtime;

	internal NodeDefinitionBuilder(Node node)
	{
		this.node = node;
	}

	public NodeDefinitionBuilder UseInput<TModel>(IInput<TModel> input)
	{
		inputs.Add(new NodeInputDefinition());
		return this;
	}

	public NodeDefinitionBuilder UseOutput<TModel>(Output<TModel> output, string name)
	{
		outputs.Add(new NodeOutputDefinition(output, name));
		return this;
	}

	public NodeDefinitionBuilder UseRuntime<TRuntime>()
		where TRuntime : NodeRuntime, new()
	{
		runtime = SharedRuntime<TRuntime>.Instance;
		return this;
	}

	/// <summary>
	/// Creates a new instance of the <see cref="NodeDefinition"/> class created from the current state of this <see cref="NodeDefinitionBuilder"/>.
	/// </summary>
	/// <returns>A new instance of the <see cref="NodeDefinition"/> class created from the current state of this <see cref="NodeDefinitionBuilder"/>.</returns>
	public NodeDefinition Build()
	{
		if (runtime == null)
		{
			throw new InvalidOperationException($"Cannot finalise construction of a {nameof(NodeDefinition)} as no {nameof(NodeRuntime)} is specified.");
		}
		return new NodeDefinition(node, runtime, inputs, outputs);
	}
}
