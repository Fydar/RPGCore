using System;

namespace Behaviour
{
	public class StatsNode : Node<StatsNode.Metadata>
	{
		public InputSocket ValueA;
		public InputSocket ValueB;
		public string TooltipFormat = "{0}";

		public override SocketMap[] Connect(GraphInstance graph, Metadata instance) => new[]
		{
			graph.Connect(ref ValueA, out instance.valueA),
			graph.Connect(ref ValueB, out instance.valueB)
		};

		public class Metadata : INodeInstance
		{
			public string seed;
			public IInput<int> valueA;
			public IInput<int> valueB;

			private Actor target;

			public void Setup(GraphInstance graph, Node parent, Actor target)
			{
				this.target = target;
				StatsNode stats = (StatsNode)parent;

				seed = Guid.NewGuid().ToString();

				valueA.OnAfterChanged += Log;
				valueB.OnAfterChanged += Log;
				target.Health.OnAfterChanged += HealthChanged;

				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("StatsNode: Setup Behaviour on " + this.target);
			}

			public void Remove()
			{
				valueA.OnAfterChanged -= Log;
				target.Health.OnAfterChanged -= HealthChanged;

				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("StatsNode: Removed Behaviour on " + target);
			}

			private void Log()
			{
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.WriteLine("StatsNode: New value of " + (valueA.Value + valueB.Value).ToString() + " on " + target);
			}

			private void HealthChanged()
			{
				Console.ForegroundColor = ConsoleColor.DarkGray;
				Console.WriteLine("StatsNode: Owner taking damage. Update Value: " + (valueA.Value + valueB.Value));
			}
		}
	}
}
