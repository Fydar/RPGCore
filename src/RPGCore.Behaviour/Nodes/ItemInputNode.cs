namespace RPGCore.Behaviour
{
	public sealed class ItemInputNode : Node<ItemInputNode>
	{
		public OutputSocket Character;

		public override Instance Create () => new ItemInputInstance ();

		public class ItemInputInstance : Instance, IInputNode<DemoPlayer>
		{
			public Output<DemoPlayer> Character;

			public override InputMap[] Inputs (IGraphConnections graph, ItemInputNode node) => null;

			public override OutputMap[] Outputs (IGraphConnections graph, ItemInputNode node) => new[]
			{
				graph.Connect (ref node.Character, ref Character)
			};

			public override void Setup (IGraphInstance graph)
			{
			}

			public override void Remove ()
			{
			}

			public void OnReceiveInput (DemoPlayer input)
			{
				Character.Value = input;
			}
		}
	}
}
