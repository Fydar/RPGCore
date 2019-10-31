namespace RPGCore.Behaviour
{
	public sealed class GetStatNode : Node<GetStatNode>
	{
		public InputSocket Character;

		public OutputSocket Output;

		public override Instance Create () => new GetStatInstance ();

		public class GetStatInstance : Instance
		{
			public Input<DemoPlayer> Character;

			public Output<int> Output;

			public override InputMap[] Inputs (IGraphConnections connections) => new[]
			{
				connections.Connect (ref Node.Character, ref Character),
			};

			public override OutputMap[] Outputs (IGraphConnections connections) => new[]
			{
				connections.Connect (ref Node.Output, ref Output)
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
