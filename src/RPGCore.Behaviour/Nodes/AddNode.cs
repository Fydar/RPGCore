using System;

namespace RPGCore.Behaviour
{
	public sealed class AddNode : Node<AddNode>
	{
		public InputSocket ValueA;
		public InputSocket ValueB;

		public OutputSocket Output;

		public override Instance Create () => new AddInstance ();

		public class AddInstance : Instance
		{
			public Input<float> ValueA;
			public Input<float> ValueB;

			public Output<float> Output;

			public override InputMap[] Inputs (IGraphConnections connections) => new[]
			{
				connections.Connect (ref Node.ValueA, ref ValueA),
				connections.Connect (ref Node.ValueB, ref ValueB)
			};

			public override OutputMap[] Outputs (IGraphConnections connections) => new[]
			{
				connections.Connect (ref Node.Output, ref Output)
			};

			public override void Setup ()
			{
			}

			public override void OnInputChanged ()
			{
				float value = ValueA.Value + ValueB.Value;

				// Console.ForegroundColor = ConsoleColor.DarkGray;
				// Console.WriteLine ($"[{Node.Id}]: Adding {ValueA.Value} + {ValueB.Value} = {value}");
				// Console.ResetColor ();

				Output.Value = value;
			}

			public override void Remove ()
			{
			}
		}
	}
}
