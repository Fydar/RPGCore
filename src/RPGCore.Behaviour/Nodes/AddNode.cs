namespace RPGCore.Behaviour
{
	public sealed class AddNode : Node<AddNode, AddNode.AddInstance>
	{
		public InputSocket ValueA;
		public InputSocket ValueB;

		public OutputSocket Output;

		public override InputMap[] Inputs (IGraphConnections graph, AddInstance instance) => new[]
		{
			graph.Connect(ref ValueA, ref instance.ValueA),
			graph.Connect(ref ValueB, ref instance.ValueB)
		};

		public override OutputMap[] Outputs (IGraphConnections graph, AddInstance instance) => new[]
		{
			graph.Connect(ref Output, ref instance.Output)
		};

		public override AddInstance Create () => new AddInstance ();

		public class AddInstance : Instance
		{
			public Input<float> ValueA;
			public Input<float> ValueB;

			public Output<float> Output;

			public override void Setup (IGraphInstance graph)
			{
			}

			public override void OnInputChanged ()
			{
				Output.Value = ValueA.Value + ValueB.Value;
			}

			public override void Remove ()
			{
			}
		}
	}
}
