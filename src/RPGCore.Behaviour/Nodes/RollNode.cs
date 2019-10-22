using System;

namespace RPGCore.Behaviour
{
	public struct ExtraData
	{
		public int Hi;
		public bool Goodbyte;
	}

	public sealed class RollNode : Node<RollNode, RollNode.RollInstance>
	{
		public OutputSocket Output = new OutputSocket ();
		public string TooltipFormat = "{0}";
		public int MinValue = 2;
		public int MaxValue = 12;
		public ExtraData Data;

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

			private Actor Target;

			public override void Setup (IGraphInstance graph, Actor target)
			{
				Target = target;

				while (Seed == 0)
				{
					Seed = new Random ().Next ();
				}

				int newValue = new Random (Seed).Next (Node.MinValue, Node.MaxValue);

				Console.ForegroundColor = ConsoleColor.DarkGreen;
				Console.WriteLine ("RollNode: Output set to " + newValue);

				Output.Value = newValue;
			}

			public override void Remove ()
			{
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.WriteLine ("RollNode: Removed Behaviour on " + Target);
			}

			public override void OnInputChanged ()
			{

			}
		}
	}
}
