using System;

namespace RPGCore.Behaviour
{
	public sealed class AddNode : Node<AddNode, AddNode.Metadata>
	{
		public InputSocket ValueA;
		public InputSocket ValueB;

		public OutputSocket Output;

		public override InputMap[] Inputs (IGraphConnections graph, Metadata instance) => new[]
		{
			graph.Connect(ref ValueA, ref instance.valueA),
			graph.Connect(ref ValueB, ref instance.valueB)
		};

		public override OutputMap[] Outputs (IGraphConnections graph, Metadata instance) => new[]
		{
			graph.Connect(ref Output, ref instance.Output)
		};

		public sealed class Metadata : Instance
		{
			public Input<float> valueA;
			public Input<float> valueB;

			public Output<float> Output;

			private Actor target;

			public override void Setup (IGraphInstance graph, Actor target)
			{
				this.target = target;

				valueA.OnAfterChanged += Log;
				valueB.OnAfterChanged += Log;
				target.Health.Handlers[this] += new LogOnChanged (this);

				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine ($"StatsNode: Setup Behaviour on {target}");

				Console.WriteLine ($"StatsNode: Connected to {graph.GetSource (valueA)}");
				Console.WriteLine ($"StatsNode: Connected to {graph.GetSource (valueB)}");
			}

			public override void OnInputChanged ()
			{
				Output.Value = valueA.Value + valueB.Value;
			}

			public override void Remove ()
			{
				target.Health.Handlers[this].Clear ();
				valueA.OnAfterChanged -= Log;

				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine ($"StatsNode: Removed Behaviour on {target}");
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
