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
		public override void OnEnable(RuntimeNode<IterateNode> runtimeNode)
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

		public override void OnDisable(RuntimeNode<IterateNode> runtimeNode)
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

		public override void OnInputChanged(RuntimeNode<IterateNode> runtimeNode)
		{
			UpdateIterations(runtimeNode);
		}

		private static void UpdateIterations(RuntimeNode<IterateNode> runtimeNode)
		{
			runtimeNode.UseInput(runtimeNode.Node.Iterations, out var iterations);

			ref var data = ref runtimeNode.GetComponent<IterateNodeData>();

			if (data.Instances == null)
			{
				data.Instances = new List<GraphRuntimeData>();
			}
			if (data.InstanceRuntimes == null)
			{
				data.InstanceRuntimes = new List<GraphRuntime>();
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

				var newInstanceData = graphDefinition.CreateRuntimeData();
				var newInstanceRuntime = runtimeNode.GraphRuntime.GraphEngine.CreateGraphRuntime(
					graphDefinition, newInstanceData);

				data.InstanceRuntimes.Add(newInstanceRuntime);
				data.Instances.Add(newInstanceData);

				using var mutate = newInstanceRuntime.Mutate();
				mutate.Enable();
			}
		}
	}

	public struct IterateNodeData : IRuntimeNodeComponent
	{
		internal List<GraphRuntime> InstanceRuntimes { get; set; }

		public List<GraphRuntimeData> Instances { get; set; }
	}
}
