﻿using RPGCore.Behaviour;

namespace RPGCore.Documentation.Samples.RPGCore.Behaviour.AddNodeSample;

#region node
public class AddNode : Node
{
	public IInput<int> ValueA { get; init; } = new DefaultInput<int>(10);
	public IInput<int> ValueB { get; init; } = new DefaultInput<int>(10);
	public Output<int> Output { get; init; } = new Output<int>();

	public override NodeDefinition CreateDefinition()
	{
		return NodeDefinition.Create(this)
			.UseInput(ValueA)
			.UseInput(ValueB)
			.UseOutput(Output, nameof(Output))
			.UseRuntime<AddNodeRuntime>()
			.Build();
	}

	public class AddNodeRuntime : NodeRuntime<AddNode>
	{
		public override void OnEnable(RuntimeNode<AddNode> runtimeNode)
		{
			runtimeNode.UseInput(runtimeNode.Node.ValueA, out var valueA);
			runtimeNode.UseInput(runtimeNode.Node.ValueB, out var valueB);
			runtimeNode.UseOutput(runtimeNode.Node.Output, out var output);

			output.Value = valueA.Value + valueB.Value;
		}

		public override void OnInputChanged(RuntimeNode<AddNode> runtimeNode)
		{
			runtimeNode.UseInput(runtimeNode.Node.ValueA, out var valueA);
			runtimeNode.UseInput(runtimeNode.Node.ValueB, out var valueB);
			runtimeNode.UseOutput(runtimeNode.Node.Output, out var output);

			output.Value = valueA.Value + valueB.Value;
		}
	}
}
#endregion
