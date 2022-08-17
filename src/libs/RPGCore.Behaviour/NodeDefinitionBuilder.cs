using RPGCore.Behaviour.Internal;
using System;
using System.Collections.Generic;

namespace RPGCore.Behaviour;

public sealed class NodeDefinitionBuilder
{
	private readonly Node node;
	private readonly List<Type> components = new();
	private readonly List<NodeDefinitionBuilderInput> inputBuilders = new();
	private readonly List<NodeDefinitionBuilderOutput> outputBuilders = new();
	private NodeRuntime? runtime;

	internal NodeDefinitionBuilder(Node node)
	{
		this.node = node;
	}

	public NodeDefinitionBuilder UseComponent<TComponent>()
		where TComponent : struct, INodeComponent
	{
		components.Add(typeof(TComponent));
		return this;
	}

	public NodeDefinitionBuilder UseInput<TModel>(IInput<TModel> input)
	{
		inputBuilders.Add(new NodeDefinitionBuilderInput(input));
		return this;
	}

	public NodeDefinitionBuilder UseOutput<TModel>(Output<TModel> output, string name)
	{
		outputBuilders.Add(new NodeDefinitionBuilderOutput(output, name));
		return this;
	}

	public NodeDefinitionBuilder UseRuntime<TRuntime>()
		where TRuntime : NodeRuntime, new()
	{
		runtime = SharedRuntime<TRuntime>.Instance;
		return this;
	}

	/// <summary>
	/// Creates a new instance of the <see cref="GraphDefinitionNode"/> class created from the current state of this <see cref="NodeDefinitionBuilder"/>.
	/// </summary>
	/// <returns>A new instance of the <see cref="GraphDefinitionNode"/> class created from the current state of this <see cref="NodeDefinitionBuilder"/>.</returns>
	public NodeDefinition Build()
	{
		if (runtime == null)
		{
			throw new InvalidOperationException($"Cannot finalise construction of a {nameof(GraphDefinitionNode)} as no {nameof(NodeRuntime)} is specified.");
		}

		var nodeDefinitionOutputs = new NodeDefinitionOutput[outputBuilders.Count];
		for (int i = 0; i < outputBuilders.Count; i++)
		{
			var outputBuilder = outputBuilders[i];

			nodeDefinitionOutputs[i] = new NodeDefinitionOutput(
				outputBuilder.output,
				$"{node.Id}.{outputBuilder.Name}");
		}

		var nodeDefinitionInputs = new List<NodeDefinitionInput>();
		foreach (var inputBuilder in inputBuilders)
		{
			nodeDefinitionInputs.Add(new NodeDefinitionInput(inputBuilder.input));
		}

		return new NodeDefinition(
			node,
			runtime,
			components.ToArray(),
			nodeDefinitionInputs.ToArray(),
			nodeDefinitionOutputs);
	}
}
