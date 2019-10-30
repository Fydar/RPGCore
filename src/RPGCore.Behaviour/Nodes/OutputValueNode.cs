using System;

namespace RPGCore.Behaviour
{
	public sealed class OutputValueNode : Node<OutputValueNode>
	{
		public InputSocket Value;

		public override Instance Create () => new OutputValueInstance ();

		public class OutputValueInstance : Instance
		{
			public Input<float> Value;

			public override InputMap[] Inputs (IGraphConnections graph, OutputValueNode node) => new[]
			{
				graph.Connect (ref node.Value, ref Value),
			};

			public override OutputMap[] Outputs (IGraphConnections graph, OutputValueNode node) => null;

			public override void Setup (IGraphInstance graph)
			{
			}

			public override void OnInputChanged ()
			{
				Console.ForegroundColor = ConsoleColor.DarkGray;
				Console.WriteLine ($"[{Node.Id}]: Outputting value {Value.Value}");
				Console.ResetColor ();
			}

			public override void Remove ()
			{
			}
		}
	}
}
