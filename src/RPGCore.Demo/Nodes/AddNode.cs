using RPGCore.Behaviour;

namespace RPGCore.Demo.Nodes
{
	public sealed class AddNode : NodeTemplate<AddNode>
	{
		public InputSocket ValueA;
		public InputSocket ValueB;

		public OutputSocket Output;

		public override Instance Create() => new AddInstance ();

		public class AddInstance : Instance
		{
			public Input<float> ValueA;
			public Input<float> ValueB;

			public Output<float> Output;

			public override InputMap[] Inputs(ConnectionMapper connections) => new[]
			{
				connections.Connect (ref Template.ValueA, ref ValueA),
				connections.Connect (ref Template.ValueB, ref ValueB)
			};

			public override OutputMap[] Outputs(ConnectionMapper connections) => new[]
			{
				connections.Connect (ref Template.Output, ref Output)
			};

			public override void Setup()
			{
			}

			public override void OnInputChanged()
			{
				float value = ValueA.Value + ValueB.Value;

				// Console.ForegroundColor = ConsoleColor.DarkGray;
				// Console.WriteLine ($"[{Node.Id}]: Adding {ValueA.Value} + {ValueB.Value} = {value}");
				// Console.ResetColor ();

				Output.Value = value;
			}

			public override void Remove()
			{
			}
		}
	}
}
