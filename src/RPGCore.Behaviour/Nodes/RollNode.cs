using System;

namespace RPGCore.Behaviour
{
	public sealed class RollNode : Node<RollNode>
	{
		public OutputSocket Output = new OutputSocket ();
		public int MinValue = 2;
		public int MaxValue = 12;

		public override Instance Create () => new RollInstance ();

		public sealed class RollInstance : Instance
		{
			public int Seed;

			public Output<int> Output;

			public override InputMap[] Inputs (IGraphConnections connections) => null;

			public override OutputMap[] Outputs (IGraphConnections connections) => new[]
			{
				connections.Connect (ref Node.Output, ref Output),
			};

			public override void Setup ()
			{
				while (Seed == 0)
				{
					Seed = new Random ().Next ();
				}

				int newValue = new Random (Seed).Next (Node.MinValue, Node.MaxValue);

				Console.ForegroundColor = ConsoleColor.DarkGreen;
				Console.WriteLine ("RollNode: Output set to " + newValue + $" outputting to {string.Join (", ", Graph.GetSource (Output))}");

				Output.Value = newValue;
			}

			public override void Remove ()
			{
			}

			public override void OnInputChanged ()
			{
			}
		}
	}
}
