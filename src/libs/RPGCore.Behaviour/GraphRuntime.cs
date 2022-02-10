namespace RPGCore.Behaviour;

public struct GraphRuntime
{
	public GraphEngine Engine { get; }
	public GraphDefinition GraphDefinition { get; }
	public GraphRuntimeData Data { get; }

	internal GraphRuntime(
		GraphEngine engine,
		GraphDefinition graphDefinition,
		GraphRuntimeData data)
	{
		Engine = engine;
		GraphDefinition = graphDefinition;
		Data = data;
	}

	public void Enable()
	{
		for (int i = 0; i < GraphDefinition.NodeDefinitions.Count; i++)
		{
			var nodeDefinition = GraphDefinition.NodeDefinitions[i];
			nodeDefinition.Runtime.OnEnable(new RuntimeNode(this, nodeDefinition.Node));
		}
	}

	public void Disable()
	{
		for (int i = 0; i < GraphDefinition.NodeDefinitions.Count; i++)
		{
			var nodeDefinition = GraphDefinition.NodeDefinitions[i];
			nodeDefinition.Runtime.OnDisable(new RuntimeNode(this, nodeDefinition.Node));
		}
	}

	public RuntimeNode<TNode>? GetNode<TNode>()
		where TNode : Node
	{
		foreach (var node in GraphDefinition.NodeDefinitions)
		{
			if (node is TNode typedNode)
			{
				return new RuntimeNode<TNode>(this, typedNode);
			}
		}
		return null;
	}
}
