using System;

namespace Behaviour
{
	public class StatsNode : Node<StatsNode.Metadata>
	{
		public InputSocket ValueA;
		public InputSocket ValueB;
		public string TooltipFormat = "{0}";

		public override InputMap[] Inputs(IGraphInstance graph, Metadata instance) => new[]
		{
			graph.Connect(ref ValueA, out instance.valueA),
			graph.Connect(ref ValueB, out instance.valueB)
		};

		public override OutputMap[] Outputs(IGraphInstance graph, Metadata instance) => null;

		public class Metadata : INodeInstance
		{
			public IInput<int> valueA;
			public IInput<int> valueB;

			public string seed;
			private Actor target;

			public void Setup(IGraphInstance graph, Node parent, Actor target)
			{
				this.target = target;
				StatsNode stats = (StatsNode)parent;

				seed = Guid.NewGuid().ToString();

				valueA.OnAfterChanged += Log;
				valueB.OnAfterChanged += Log;
				target.Health.Handlers[this] += new LogOnChanged(this);

				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("StatsNode: Setup Behaviour on " + this.target);
			}

			public void Remove()
			{
				target.Health.Handlers[this].Clear();
				valueA.OnAfterChanged -= Log;

				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("StatsNode: Removed Behaviour on " + target);
			}

			struct LogOnChanged : IEventFieldHandler
			{
				public Metadata Meta;

				public LogOnChanged(Metadata meta)
				{
					Meta = meta;
				}

				public void OnBeforeChanged()
				{
					
				}

				public void OnAfterChanged()
				{
					Console.ForegroundColor = ConsoleColor.Gray;
					Console.WriteLine("StatsNode: New value of " + (Meta.valueA.Value + Meta.valueB.Value).ToString() + " on " + Meta.target);
				}
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
