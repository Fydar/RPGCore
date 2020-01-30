using RPGCore.Behaviour;

namespace RPGCore.Demo.Nodes
{
	public sealed class GetStatNode : NodeTemplate<GetStatNode>
	{
		public InputSocket Character;

		public OutputSocket Output;

		public override Instance Create() => new GetStatInstance();

		public class GetStatInstance : Instance
		{
			public Input<DemoPlayer> Character;

			public Output<int> Output;

			public override InputMap[] Inputs(ConnectionMapper connections) => new[]
			{
				connections.Connect (ref Template.Character, ref Character),
			};

			public override OutputMap[] Outputs(ConnectionMapper connections) => new[]
			{
				connections.Connect (ref Template.Output, ref Output)
			};

			public override void Setup()
			{
				var statValue = Character.Watch(c => c?.Health);

				Output.StartMirroring(statValue);
			}

			public override void Remove()
			{
				Output.StopMirroring();
			}
		}
	}
}
