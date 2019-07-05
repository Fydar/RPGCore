using System;

namespace RPGCore.Behaviour
{
	public class AddNode : Node<AddNode.Metadata>
	{
		public InputSocket ValueA;
		public InputSocket ValueB;

		public OutputSocket Output;

		public override InputMap[] Inputs (IGraphInstance graph, Metadata instance) => new[]
		{
			graph.Connect(ref ValueA, ref instance.valueA),
			graph.Connect(ref ValueB, ref instance.valueB)
		};

		public override OutputMap[] Outputs (IGraphInstance graph, Metadata instance) => new[]
		{
			graph.Connect(ref Output, ref instance.Output)
		};

		public class Metadata : INodeInstance
		{
			public IInput<int> valueA = new StaticValue<int>(10);
			public IInput<int> valueB = new StaticValue<int> (20);

			public IOutput<int> Output;

			private Actor target;

			public void Setup (IGraphInstance graph, Node parent, Actor target)
			{
				this.target = target;

				var stats = (AddNode)parent;

				valueA.OnAfterChanged += Log;
				valueB.OnAfterChanged += Log;
				target.Health.Handlers[this] += new LogOnChanged (this);

				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine ("StatsNode: Setup Behaviour on " + this.target);
			}

			public void Remove ()
			{
				target.Health.Handlers[this].Clear ();
				valueA.OnAfterChanged -= Log;

				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine ("StatsNode: Removed Behaviour on " + target);
			}

			private struct LogOnChanged : IEventFieldHandler
			{
				public Metadata Meta;

				public LogOnChanged (Metadata meta)
				{
					Meta = meta;
				}

				public void OnBeforeChanged ()
				{

				}

				public void OnAfterChanged ()
				{
					Console.ForegroundColor = ConsoleColor.Gray;
					Console.WriteLine ("StatsNode: Damaged on " + (Meta.valueA.Value + Meta.valueB.Value).ToString () + " on " + Meta.target);
				}

				public void Dispose ()
				{

				}
			}

			private void Log ()
			{
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.WriteLine ("StatsNode: New value of " + (valueA.Value + valueB.Value).ToString () + " on " + target);
			}
		}
	}
}
