using System;
using System.Collections.Generic;

namespace RPGCore.Behaviour.UnitTests.Nodes;

public class IterateNode : Node
{
	public IInput<int> Iterations { get; set; } = new DefaultInput<int>(1);
	public Graph Graph { get; set; } = new Graph(Array.Empty<Node>());

	public override NodeDefinition CreateDefinition()
	{
		return NodeDefinition.Create(this)
			.UseInput(Iterations)
			.UseRuntime<IterateNodeRuntime>()
			.Build();
	}

	public class IterateNodeRuntime : NodeRuntime<IterateNode>
	{
		public override void OnEnable(RuntimeNode<IterateNode> runtimeNode)
		{
			UpdateIterations(runtimeNode);
		}

		public override void OnInputChanged(RuntimeNode<IterateNode> runtimeNode)
		{
			UpdateIterations(runtimeNode);
		}

		private void UpdateIterations(RuntimeNode<IterateNode> runtimeNode)
		{
			runtimeNode.UseInput(runtimeNode.Node.Iterations, out var iterations);

			if (iterations.HasChanged)
			{
				ref var data = ref runtimeNode.GetNodeInstanceData<IterateNodeData>();

				// Remove unused instances
				while (data.Instances.Count > iterations.Value)
				{
					data.Instances.RemoveAt(data.Instances.Count - 1);
				}

				// Add additional instances
				while (data.Instances.Count < iterations.Value)
				{
					// TODO: Create child graph...
					// data.Instances.Add(Graph.CreateRunnerData());
				}
			}
		}
	}

	public struct IterateNodeData : INodeData
	{
		public List<GraphRuntimeData> Instances { get; set; }
	}
}
