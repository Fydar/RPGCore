using RPGCore.Behaviour;
using System;

namespace RPGCore.Demo.Inventory.Nodes
{
	public sealed class RollNode : NodeTemplate<RollNode>
	{
		public OutputSocket Output = new OutputSocket();
		public int MinValue = 2;
		public int MaxValue = 12;

		public override Instance Create()
		{
			return new RollInstance();
		}

		public sealed class RollInstance : Instance
		{
			public int Seed;

			public Output<int> Output;

			public override InputMap[] Inputs(ConnectionMapper connections) => null;

			public override OutputMap[] Outputs(ConnectionMapper connections) => new[]
			{
				connections.Connect (ref Template.Output, ref Output),
			};

			public override void Setup()
			{
				while (Seed == 0)
				{
					Seed = new Random().Next();
				}

				int newValue = new Random(Seed).Next(Template.MinValue, Template.MaxValue);

				Console.ForegroundColor = ConsoleColor.DarkGreen;
				Console.WriteLine("RollNode: Output set to " + newValue + $" outputting to {string.Join(", ", Graph.GetSource(Output))}");

				Output.Value = newValue;
			}

			public override void Remove()
			{
			}

			public override void OnInputChanged()
			{
			}
		}
	}
}
