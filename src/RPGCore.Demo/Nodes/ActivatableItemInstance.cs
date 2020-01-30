using RPGCore.Behaviour;

namespace RPGCore.Demo.Nodes
{
	public sealed class ActivatableItemNode : NodeTemplate<ActivatableItemNode>
	{
		public OutputSocket Output = new OutputSocket ();


		public override Instance Create() => new ActivatableItemInstance ();

		public sealed class ActivatableItemInstance : Instance
		{
			public Output<int> Output;

			public override InputMap[] Inputs(ConnectionMapper connections) => null;

			public override OutputMap[] Outputs(ConnectionMapper connections) => new[]
			{
				connections.Connect(ref Template.Output, ref Output),
			};

			public override void Setup()
			{

			}

			public override void Remove()
			{

			}

			public override void OnInputChanged()
			{

			}
		}
	}
}
