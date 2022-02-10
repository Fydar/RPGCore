namespace RPGCore.Behaviour.UnitTests.Nodes;

public class DurabilityNode : Node
{
	public IInput<int> BaseDurability { get; init; } = new DefaultInput<int>(10);
	public Output<int> CurrentDurability { get; init; } = new Output<int>();

	public override NodeDefinition CreateDefinition()
	{
		return NodeDefinition.Create(this)
			.UseInput(BaseDurability)
			.UseOutput(CurrentDurability, nameof(CurrentDurability))
			.UseRuntime<DurabilityNodeRuntime>()
			.Build();
	}

	public class DurabilityNodeRuntime : NodeRuntime<DurabilityNode>
	{
		public override void OnInputChanged(RuntimeNode<DurabilityNode> runtimeNode)
		{
			runtimeNode.UseInput(runtimeNode.Node.BaseDurability, out var baseDurability);
			runtimeNode.UseOutput(runtimeNode.Node.CurrentDurability, out var currentDurability);

			currentDurability.Value += baseDurability.Value;

			ref var data = ref runtimeNode.GetNodeInstanceData<DurabilityNodeData>();

			data.MaxDurability += 1;
		}
	}

	public struct DurabilityNodeData : INodeData
	{
		public int MaxDurability { get; set; }
	}
}
