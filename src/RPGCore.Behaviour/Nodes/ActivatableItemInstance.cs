namespace RPGCore.Behaviour
{
	public sealed class ActivatableItemNode : Node<ActivatableItemNode, ActivatableItemNode.ActivatableItemInstance>
	{
		public OutputSocket Output = new OutputSocket ();

		public override InputMap[] Inputs (IGraphConnections graph, ActivatableItemInstance instance) => null;

		public override OutputMap[] Outputs (IGraphConnections graph, ActivatableItemInstance instance) => new[]
		{
			graph.Connect(ref Output, ref instance.Output),
		};

		public override ActivatableItemInstance Create () => new ActivatableItemInstance ();

		public sealed class ActivatableItemInstance : Instance
		{
			public Output<int> Output;

			public override void Setup (IGraphInstance graph)
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
