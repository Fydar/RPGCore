namespace RPGCore.Behaviour;

public readonly ref struct GraphInstanceMutationScope
{
	public GraphInstance GraphInstance { get; }

	public GraphInstanceMutationScope(
		GraphInstance graphRuntime)
	{
		GraphInstance = graphRuntime;
	}

	public void Dispose()
	{
		for (int i = 0; i < GraphInstance.GraphEngine.nodes.Length; i++)
		{
			var graphEngineNode = GraphInstance.GraphEngine.Nodes[i];

			ref var nodeData = ref GraphInstance.GraphRuntimeData.Nodes[i];

			if (nodeData.Outputs != null)
			{
				bool executedOnChanged = false;
				foreach (var output in nodeData.Outputs.Values)
				{
					if (output.HasChanged)
					{
						if (!executedOnChanged)
						{
							graphEngineNode.Runtime.OnInputChanged(new GraphInstanceNode(GraphInstance, graphEngineNode.Node));

							executedOnChanged = true;
						}

						output.HasChanged = false;
					}
				}
			}

			bool hasAnyInputChanged = false;
			// foreach (var input in graphEngineNode.ConnectedInputDefinitions)
			{
				//  if (input.HasChanged)
				//  {
				//  	if (!hasAnyInputChanged)
				//  	{
				//  		nodeDefinition.Runtime.OnInputChanged(new GraphInstanceNode(GraphRuntime, nodeDefinition.Node));
				//  
				//  		hasAnyInputChanged = true;
				//  	}
				//  
				//  
				//  	input.HasChanged = false;
				//  }
			}
		}
	}

	public void Enable()
	{
		for (int i = 0; i < GraphInstance.GraphEngine.Nodes.Count; i++)
		{
			var graphEngineNode = GraphInstance.GraphEngine.Nodes[i];
			graphEngineNode.Runtime.OnEnable(new GraphInstanceNode(GraphInstance, graphEngineNode.Node));
		}
	}

	public void Disable()
	{
		for (int i = 0; i < GraphInstance.GraphEngine.Nodes.Count; i++)
		{
			var graphEngineNode = GraphInstance.GraphEngine.Nodes[i];
			graphEngineNode.Runtime.OnDisable(new GraphInstanceNode(GraphInstance, graphEngineNode.Node));
		}
	}

	public bool TryGetNode<TNode>(out GraphInstanceNode<TNode> node)
		where TNode : Node
	{
		for (int i = 0; i < GraphInstance.GraphEngine.Nodes.Count; i++)
		{
			var graphEngineNode = GraphInstance.GraphEngine.Nodes[i];

			if (graphEngineNode.Node is TNode typedNode)
			{
				node = new GraphInstanceNode<TNode>(GraphInstance, typedNode);
				return true;
			}
		}
		node = default;
		return false;
	}
}
