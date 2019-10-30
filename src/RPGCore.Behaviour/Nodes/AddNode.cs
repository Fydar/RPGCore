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

			public override InputMap[] Inputs (IGraphConnections graph, AddNode node) => new[]
			{
				graph.Connect (ref node.ValueA, ref ValueA),
				graph.Connect (ref node.ValueB, ref ValueB)
			};

			public override OutputMap[] Outputs (IGraphConnections graph, AddNode node) => new[]
			{
				graph.Connect (ref node.Output, ref Output)
			};

			public override void Setup (IGraphInstance graph)
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
