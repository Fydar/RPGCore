namespace RPGCore.Behaviour.UnitTests.Nodes;

public class AddNode : Node
{
	public IInput<int> ValueA { get; set; } = new DefaultInput<int>(10);
	public IInput<int> ValueB { get; set; } = new DefaultInput<int>(10);
	public Output<int> Output { get; set; } = new Output<int>();

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
		public override void OnEnable(GraphInstanceNode<AddNode> runtimeNode)
		{
			runtimeNode.OpenInput(runtimeNode.Node.ValueA, out var valueA);
			runtimeNode.OpenInput(runtimeNode.Node.ValueB, out var valueB);
			runtimeNode.OpenOutput(runtimeNode.Node.Output, out var output);

			output.Value = valueA.Value + valueB.Value;
		}

		public override void OnInputChanged(GraphInstanceNode<AddNode> runtimeNode)
		{
			runtimeNode.OpenInput(runtimeNode.Node.ValueA, out var valueA);
			runtimeNode.OpenInput(runtimeNode.Node.ValueB, out var valueB);
			runtimeNode.OpenOutput(runtimeNode.Node.Output, out var output);

			output.Value = valueA.Value + valueB.Value;
		}
	}
}
