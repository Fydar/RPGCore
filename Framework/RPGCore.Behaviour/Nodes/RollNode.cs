using System;

namespace RPGCore.Behaviour
{
	public struct ExtraData
	{
		public int Hi;
		public bool Goodbyte;
	}

	public class RollNode : Node<RollNode.Metadata>
	{
		public OutputSocket Output = new OutputSocket ();
		public string TooltipFormat = "{0}";
		public int MinValue = 2;
		public int MaxValue = 12;
		public ExtraData Data;

		public override InputMap[] Inputs (IGraphInstance graph, Metadata instance) => null;

		public override OutputMap[] Outputs (IGraphInstance graph, Metadata instance) => new[]
		{
			graph.Connect(ref Output, ref instance.Output),
		};

		public class Metadata : INodeInstance
		{
			public Output<int> Output;

			public int Seed;

			private Actor target;

			public void Setup (IGraphInstance graph, Node parent, Actor target)
			{
				this.target = target;
				var stats = (RollNode)parent;

				while (Seed == 0)
				{
					Seed = new Random ().Next ();
				}

				int newValue = new Random (Seed).Next (stats.MinValue, stats.MaxValue);

				Console.ForegroundColor = ConsoleColor.DarkGreen;
				Console.WriteLine ("RollNode: Output set to " + newValue);

				Output.Value = newValue;
			}

			public void Remove ()
			{
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.WriteLine ("RollNode: Removed Behaviour on " + target);
			}
		}
	}
}
