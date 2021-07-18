using RPGCore.Behaviour;
using System;

namespace RPGCore.Demo.Inventory.Nodes
{
	public sealed class OutputValueNode : NodeTemplate<OutputValueNode>
	{
		public InputSocket Value;

		public override Instance Create()
		{
			return new OutputValueInstance();
		}

		public class OutputValueInstance : Instance
		{
			public Input<float> Value;

			public override InputMap[] Inputs(ConnectionMapper connections) => new[]
			{
				connections.Connect (ref Template.Value, ref Value),
			};

			public override OutputMap[] Outputs(ConnectionMapper connections) => null;

			public override void Setup()
			{
			}

			public override void OnInputChanged()
			{
				Console.ForegroundColor = ConsoleColor.DarkGray;
				Console.WriteLine($"[{Template.Id}]: Outputting value {Value.Value}");
				Console.ResetColor();
			}

			public override void Remove()
			{
			}
		}
	}
}
