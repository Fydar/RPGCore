using System;

namespace RPGCore.Behaviour
{
	public sealed class AddNode : Node<AddNode, AddNode.AddInstance>
	{
		public InputSocket ValueA;
		public InputSocket ValueB;

		public OutputSocket Output;

		public override InputMap[] Inputs (IGraphConnections graph, AddInstance instance) => new[]
		{
			graph.Connect(ref ValueA, ref instance.ValueA),
			graph.Connect(ref ValueB, ref instance.ValueB)
		};

		public override OutputMap[] Outputs (IGraphConnections graph, AddInstance instance) => new[]
		{
			graph.Connect(ref Output, ref instance.Output)
		};

		public override AddInstance Create () => new AddInstance ();

		public sealed class AddInstance : Instance
		{
			public Input<float> ValueA;
			public Input<float> ValueB;

			public Output<float> Output;

			private Actor Target;

			public override void Setup (IGraphInstance graph, Actor target)
			{
				Target = target;

				ValueA.OnAfterChanged += Log;
				ValueB.OnAfterChanged += Log;
				target.Health.Handlers[this] += new LogOnChanged (this);

				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine ($"StatsNode: Setup Behaviour on {target}");

				Console.WriteLine ($"StatsNode: Connected to {graph.GetSource (ValueA)}");
				Console.WriteLine ($"StatsNode: Connected to {graph.GetSource (ValueB)}");
			}

			public override void OnInputChanged ()
			{
				Output.Value = ValueA.Value + ValueB.Value;
			}

			public override void Remove ()
			{
				Target.Health.Handlers[this].Clear ();
				ValueA.OnAfterChanged -= Log;
				ValueB.OnAfterChanged -= Log;

				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine ($"StatsNode: Removed Behaviour on {Target}");
			}

			private struct LogOnChanged : IEventFieldHandler
			{
				public AddInstance Meta;

				public LogOnChanged (AddInstance meta)
				{
					Meta = meta;
				}

				public void OnBeforeChanged ()
				{

				}

				public void OnAfterChanged ()
				{
					Console.ForegroundColor = ConsoleColor.Gray;
					Console.WriteLine ("StatsNode: Damaged on " + (Meta.ValueA.Value + Meta.ValueB.Value).ToString () + " on " + Meta.Target);
				}

				public void Dispose ()
				{

				}
			}

			private void Log ()
			{
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.WriteLine ("StatsNode: New value of " + (ValueA.Value + ValueB.Value).ToString () + " on " + Target);
			}
		}
	}
}
