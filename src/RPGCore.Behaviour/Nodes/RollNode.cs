using System;

namespace RPGCore.Behaviour
{
	public sealed class RollNode : Node<RollNode, RollNode.RollInstance>
	{
		public OutputSocket Output = new OutputSocket ();
		public int MinValue = 2;
		public int MaxValue = 12;

		public override InputMap[] Inputs (IGraphConnections graph, RollInstance instance) => null;

		public override OutputMap[] Outputs (IGraphConnections graph, RollInstance instance) => new[]
		{
			graph.Connect(ref Output, ref instance.Output),
		};

		public override RollInstance Create () => new RollInstance ();

		public sealed class RollInstance : Instance
		{
			public int Seed;

			public Output<int> Output;

			public override void Setup (IGraphInstance graph)
			{
				while (Seed == 0)
				{
					Seed = new Random ().Next ();
				}

				int newValue = new Random (Seed).Next (Node.MinValue, Node.MaxValue);

				Console.ForegroundColor = ConsoleColor.DarkGreen;
				Console.WriteLine ("RollNode: Output set to " + newValue + $" outputting to {string.Join (", ", graph.GetSource (Output))}");

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
