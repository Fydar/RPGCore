namespace RPGCore.Behaviour
{
	public sealed class ActivatableItemNode : Node<ActivatableItemNode>
	{
		public OutputSocket Output = new OutputSocket ();


		public override INodeInstance Create () => new ActivatableItemInstance ();

		public sealed class ActivatableItemInstance : Instance
		{
			public Output<int> Output;

			public override InputMap[] Inputs (IGraphConnections graph, ActivatableItemNode node) => null;

			public override OutputMap[] Outputs (IGraphConnections graph, ActivatableItemNode node) => new[]
			{
				graph.Connect(ref node.Output, ref Output),
			};

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
