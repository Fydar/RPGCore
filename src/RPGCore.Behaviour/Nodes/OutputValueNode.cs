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

			public override InputMap[] Inputs (IGraphConnections connections) => new[]
			{
				connections.Connect (ref Node.Value, ref Value),
			};

			public override OutputMap[] Outputs (IGraphConnections connections) => null;

			public override void Setup ()
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
