using RPGCore.Behaviour;

namespace RPGCore.Demo.Nodes
{
	public sealed class ItemInputNode : Node<ItemInputNode>
	{
		public OutputSocket Character;

		public override Instance Create() => new ItemInputInstance ();

		public class ItemInputInstance : Instance, IInputNode<DemoPlayer>
		{
			public Output<DemoPlayer> Character;

			public override InputMap[] Inputs(ConnectionMapper connections) => null;

			public override OutputMap[] Outputs(ConnectionMapper connections) => new[]
			{
				connections.Connect (ref Node.Character, ref Character)
			};

			public override void Setup()
			{
			}

			public override void Remove()
			{
			}

			public void OnReceiveInput(DemoPlayer input)
			{
				Character.Value = input;
			}
		}
	}
}
