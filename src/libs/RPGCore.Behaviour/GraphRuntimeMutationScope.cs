namespace RPGCore.Behaviour;

public readonly ref struct GraphRuntimeMutationScope
{
	public GraphRuntime GraphRuntime { get; }

	public GraphRuntimeMutationScope(
		GraphRuntime graphRuntime)
	{
		GraphRuntime = graphRuntime;
	}

	public void Dispose()
	{
		for (int i = 0; i < GraphRuntime.GraphDefinition.NodeDefinitions.Count; i++)
		{
			var nodeDefinition = GraphRuntime.GraphDefinition.NodeDefinitions[i];

			ref var nodeData = ref GraphRuntime.GraphRuntimeData.Nodes[i];

			if (nodeData.Outputs != null)
			{
				bool executedOnChanged = false;
				foreach (var output in nodeData.Outputs.Values)
				{
					if (output.HasChanged)
					{
						if (!executedOnChanged)
						{
							nodeDefinition.Runtime.OnInputChanged(new RuntimeNode(GraphRuntime, nodeDefinition.Node));

							executedOnChanged = true;
						}
						

						output.HasChanged = false;
					}
				}
			}
		}
	}

	public void Enable()
	{
		for (int i = 0; i < GraphRuntime.GraphDefinition.NodeDefinitions.Count; i++)
		{
			var nodeDefinition = GraphRuntime.GraphDefinition.NodeDefinitions[i];
			nodeDefinition.Runtime.OnEnable(new RuntimeNode(GraphRuntime, nodeDefinition.Node));
		}
	}

	public void Disable()
	{
		for (int i = 0; i < GraphRuntime.GraphDefinition.NodeDefinitions.Count; i++)
		{
			var nodeDefinition = GraphRuntime.GraphDefinition.NodeDefinitions[i];
			nodeDefinition.Runtime.OnDisable(new RuntimeNode(GraphRuntime, nodeDefinition.Node));
		}
	}

	public bool TryGetNode<TNode>(out RuntimeNode<TNode> node)
		where TNode : Node
	{
		foreach (var nodeDefinition in GraphRuntime.GraphDefinition.NodeDefinitions)
		{
			if (nodeDefinition.Node is TNode typedNode)
			{
				node = new RuntimeNode<TNode>(GraphRuntime, typedNode);
				return true;
			}
		}
		node = default;
		return false;
	}
}
