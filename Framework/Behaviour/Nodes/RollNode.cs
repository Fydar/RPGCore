using System;

namespace Behaviour
{
	public class RollNode : Node<RollNode.Metadata>
	{
		public OutputSocket Output = new OutputSocket();
		public string TooltipFormat = "{0}";
		public int MinValue = 2;
		public int MaxValue = 12;

		public override SocketMap[] Connect(GraphInstance graph, Metadata instance) => new[]
		{
			graph.Connect(ref Output, out instance.output),
		};

		public class Metadata : INodeInstance
		{
			private Actor target;
			public IOutput<int> output;
			public IOutput<int> rollValue;

			public void Setup(GraphInstance graph, Node parent, Actor target)
			{
				this.target = target;
				RollNode stats = (RollNode)parent;

				int newValue = new Random().Next(stats.MinValue, stats.MaxValue);

				Console.ForegroundColor = ConsoleColor.DarkGreen;
				Console.WriteLine("RollNode: Setup Behaviour on " + this.target + " with a value of " + newValue);

				output.Value = newValue;
			}

			public void Remove()
			{
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.WriteLine("RollNode: Removed Behaviour on " + target);
			}
		}
	}
}
