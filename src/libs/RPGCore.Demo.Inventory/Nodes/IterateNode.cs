using RPGCore.Behaviour;
using System;
using System.Collections.Generic;

namespace RPGCore.Demo.Inventory.Nodes
{
	public sealed class IterateNode : NodeTemplate<IterateNode>
	{
		public InputSocket Repetitions;
		public string SubgraphId;

		public override Instance Create() => new IterateInstance();

		public class IterateInstance : Instance
		{
			public Input<float> Repetitions;

			public List<GraphInstance> SubGraphs;

			public override InputMap[] Inputs(ConnectionMapper graph) => new[]
			{
				graph.Connect (ref Template.Repetitions, ref Repetitions),
			};

			public override OutputMap[] Outputs(ConnectionMapper graph) => null;

			public override void Setup()
			{
				if (SubGraphs == null)
				{
					SubGraphs = new List<GraphInstance>();
				}
			}

			public override void OnInputChanged()
			{
				int repetitions = (int)Repetitions.Value;
				var graphToUse = Graph.Template.SubGraphs[Template.SubgraphId];

				Console.ForegroundColor = ConsoleColor.Cyan;
				Console.WriteLine($"[{Template.Id}]: Repeating {Template.SubgraphId} {repetitions} times");
				Console.ResetColor();

				for (int i = 0; i < repetitions; i++)
				{
					GraphInstance subgraphInstance;
					if (SubGraphs.Count > i)
					{
						subgraphInstance = SubGraphs[i];
					}
					else
					{
						subgraphInstance = graphToUse.Create();
						SubGraphs.Add(subgraphInstance);
					}
					subgraphInstance.Setup();
				}

				while (SubGraphs.Count > repetitions)
				{
					var removeGraph = SubGraphs[SubGraphs.Count - 1];
					SubGraphs.RemoveAt(SubGraphs.Count - 1);
					removeGraph.Remove();
				}
			}

			public override void Remove()
			{
				foreach (var removeGraph in SubGraphs)
				{
					removeGraph.Remove();
				}
			}
		}
	}
}
