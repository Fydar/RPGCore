namespace RPGCore.Behaviour
{
	public sealed class GetStatNode : Node<GetStatNode>
	{
		public InputSocket Character;

		public OutputSocket Output;

		public override INodeInstance Create () => new GetStatInstance ();

		public class GetStatInstance : Instance
		{
			public Input<DemoPlayer> Character;

			public Output<int> Output;

			public override InputMap[] Inputs (IGraphConnections graph, GetStatNode node) => new[]
			{
				graph.Connect (ref node.Character, ref Character),
			};

			public override OutputMap[] Outputs (IGraphConnections graph, GetStatNode node) => new[]
			{
				graph.Connect (ref node.Output, ref Output)
			};

			public override void Setup (IGraphInstance graph)
			{
				var statValue = Character.Watch (c => c?.Health);

				Output.StartMirroring (statValue);
			}

			public override void Remove ()
			{
				Output.StopMirroring ();
			}
		}
	}
}
