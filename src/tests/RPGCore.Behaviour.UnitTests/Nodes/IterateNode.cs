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
			.UseComponent<IterateNodeData>()
			.UseRuntime<IterateNodeRuntime>()
			.Build();
	}

	public class IterateNodeRuntime : NodeRuntime<IterateNode>
	{
		public override void OnEnable(GraphInstanceNode<IterateNode> runtimeNode)
		{
			ref var data = ref runtimeNode.GetComponent<IterateNodeData>();

			if (data.InstanceRuntimes != null)
			{
				foreach (var childInstanceRuntime in data.InstanceRuntimes)
				{
					using var mutate = childInstanceRuntime.Mutate();
					mutate.Enable();
				}
			}

			UpdateIterations(runtimeNode);
		}

		public override void OnDisable(GraphInstanceNode<IterateNode> runtimeNode)
		{
			ref var data = ref runtimeNode.GetComponent<IterateNodeData>();

			if (data.InstanceRuntimes != null)
			{
				foreach (var childInstanceRuntime in data.InstanceRuntimes)
				{
					using var mutate = childInstanceRuntime.Mutate();
					mutate.Disable();
				}
			}
		}

		public override void OnInputChanged(GraphInstanceNode<IterateNode> runtimeNode)
		{
			UpdateIterations(runtimeNode);
		}

		private static void UpdateIterations(GraphInstanceNode<IterateNode> runtimeNode)
		{
			runtimeNode.OpenInput(runtimeNode.Node.Iterations, out var iterations);

			ref var data = ref runtimeNode.GetComponent<IterateNodeData>();

			if (data.Instances == null)
			{
				data.Instances = new List<GraphInstanceData>();
			}
			if (data.InstanceRuntimes == null)
			{
				data.InstanceRuntimes = new List<GraphInstance>();
			}

			// Remove unused instances
			while (data.Instances.Count > iterations.Value)
			{
				data.Instances.RemoveAt(data.Instances.Count - 1);
			}

			// Add additional instances
			while (data.Instances.Count < iterations.Value)
			{
				var graphDefinition = runtimeNode.Node.Graph.CreateDefinition();
				var graphEngine = graphDefinition.CreateEngine();

				var newInstanceData = graphEngine.CreateInstanceData();
				var newInstanceRuntime = runtimeNode.GraphRuntime.BehaviourEngine.CreateGraphRuntime(
					graphEngine, newInstanceData);

				data.InstanceRuntimes.Add(newInstanceRuntime);
				data.Instances.Add(newInstanceData);

				using var mutate = newInstanceRuntime.Mutate();
				mutate.Enable();
			}
		}
	}

	public struct IterateNodeData : INodeComponent
	{
		internal List<GraphInstance> InstanceRuntimes { get; set; }

		public List<GraphInstanceData> Instances { get; set; }
	}
}
