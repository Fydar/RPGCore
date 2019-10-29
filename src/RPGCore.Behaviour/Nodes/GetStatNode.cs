namespace RPGCore.Behaviour
{
	public sealed class GetStatNode : Node<GetStatNode, GetStatNode.GetStatInstance>
	{
		public InputSocket Character;

		public OutputSocket Output;

		public override InputMap[] Inputs (IGraphConnections graph, GetStatInstance instance) => new[]
		{
			graph.Connect (ref Character, ref instance.Character),
		};

		public override OutputMap[] Outputs (IGraphConnections graph, GetStatInstance instance) => new[]
		{
			graph.Connect (ref Output, ref instance.Output)
		};

		public override GetStatInstance Create () => new GetStatInstance ();

		public class GetStatInstance : Instance
		{
			public Input<DemoPlayer> Character;

			public Output<int> Output;

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
