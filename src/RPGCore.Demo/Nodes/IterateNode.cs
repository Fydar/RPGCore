using RPGCore.Behaviour;
using System;
using System.Collections.Generic;

namespace RPGCore.Demo.Nodes
{
	public sealed class IterateNode : Node<IterateNode>
	{
		public InputSocket Repetitions;
		public string SubgraphId;

		public override Instance Create() => new IterateInstance ();

		public class IterateInstance : Instance
		{
			public Input<float> Repetitions;

			public List<GraphInstance> SubGraphs;

			public override InputMap[] Inputs(IGraphConnections graph) => new[]
			{
				graph.Connect (ref Node.Repetitions, ref Repetitions),
			};

			public override OutputMap[] Outputs(IGraphConnections graph) => null;

			public override void Setup()
			{
				SubGraphs = new List<GraphInstance> ();
			}

			public override void OnInputChanged()
			{
				int repetitions = (int)Repetitions.Value;
				var graphToUse = Graph.Template.SubGraphs[Node.SubgraphId];

				Console.ForegroundColor = ConsoleColor.Cyan;
				Console.WriteLine ($"[{Node.Id}]: Repeating {Node.SubgraphId} {repetitions} times");
				Console.ResetColor ();

				for (int i = 0; i < repetitions; i++)
				{
					var subgraphInstance = graphToUse.Create ();
					subgraphInstance.Setup ();

					SubGraphs.Add (subgraphInstance);
				}
			}

			public override void Remove()
			{
			}
		}
	}
}
