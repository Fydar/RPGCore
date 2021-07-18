using RPGCore.Behaviour;

namespace RPGCore.Demo.Inventory.Nodes
{
	public sealed class ItemInputNode : NodeTemplate<ItemInputNode>
	{
		public OutputSocket Character;

		public override Instance Create()
		{
			return new ItemInputInstance();
		}

		public class ItemInputInstance : Instance, IInputNode<DemoPlayer>
		{
			public Output<DemoPlayer> Character;

			public override InputMap[] Inputs(ConnectionMapper connections) => null;

			public override OutputMap[] Outputs(ConnectionMapper connections) => new[]
			{
				connections.Connect (ref Template.Character, ref Character)
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
