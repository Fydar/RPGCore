namespace RPGCore.Behaviour.UnitTests.Nodes;

public class DurabilityNode : Node
{
	public IInput<int> BaseDurability { get; set; } = new DefaultInput<int>(10);
	public Output<int> CurrentDurability { get; set; } = new Output<int>();

	public override NodeDefinition CreateDefinition()
	{
		return NodeDefinition.Create(this)
			.UseInput(BaseDurability)
			.UseOutput(CurrentDurability, nameof(CurrentDurability))
			.UseComponent<DurabilityNodeComponent>()
			.UseRuntime<DurabilityNodeRuntime>()
			.Build();
	}

	private class DurabilityNodeRuntime : NodeRuntime<DurabilityNode>
	{
		public override void OnEnable(GraphInstanceNode<DurabilityNode> runtimeNode)
		{
			ref var data = ref runtimeNode.GetComponent<DurabilityNodeComponent>();

			data.MaxDurability = 50;
		}
		public override void OnInputChanged(GraphInstanceNode<DurabilityNode> runtimeNode)
		{
			runtimeNode.OpenInput(runtimeNode.Node.BaseDurability, out var baseDurability);
			runtimeNode.OpenOutput(runtimeNode.Node.CurrentDurability, out var currentDurability);

			currentDurability.Value += baseDurability.Value;

			ref var data = ref runtimeNode.GetComponent<DurabilityNodeComponent>();

			data.MaxDurability += 1;
		}
	}

	public struct DurabilityNodeComponent : INodeComponent
	{
		public int MaxDurability { get; set; }
	}
}
