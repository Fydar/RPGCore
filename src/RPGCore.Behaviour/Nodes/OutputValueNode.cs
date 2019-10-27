using System;

namespace RPGCore.Behaviour
{
	public sealed class OutputValueNode : Node<OutputValueNode, OutputValueNode.OutputValueInstance>
	{
		public InputSocket Value;

		public override InputMap[] Inputs (IGraphConnections graph, OutputValueInstance instance) => new[]
		{
			graph.Connect (ref Value, ref instance.Value),
		};

		public override OutputMap[] Outputs (IGraphConnections graph, OutputValueInstance instance) => null;

		public override OutputValueInstance Create () => new OutputValueInstance ();

		public class OutputValueInstance : Instance
		{
			public Input<float> Value;

			public override void Setup (IGraphInstance graph)
			{
			}

			public override void OnInputChanged ()
			{
				Console.WriteLine ($"[{Node.Id}]: Outputting value {Value.Value}");
			}

			public override void Remove ()
			{
			}
		}
	}
}
