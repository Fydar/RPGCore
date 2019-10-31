using RPGCore.Behaviour;

namespace RPGCore.Demo.Nodes
{
	public sealed class ActivatableItemNode : Node<ActivatableItemNode>
	{
		public OutputSocket Output = new OutputSocket ();


		public override Instance Create () => new ActivatableItemInstance ();

		public sealed class ActivatableItemInstance : Instance
		{
			public Output<int> Output;

			public override InputMap[] Inputs (IGraphConnections connections) => null;

			public override OutputMap[] Outputs (IGraphConnections connections) => new[]
			{
				connections.Connect(ref Node.Output, ref Output),
			};

			public override void Setup ()
			{

			}

			public override void Remove ()
			{

			}

			public override void OnInputChanged ()
			{

			}
		}
	}
}
