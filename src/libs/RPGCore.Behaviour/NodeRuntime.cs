namespace RPGCore.Behaviour;

public abstract class NodeRuntime
{
	internal abstract void OnEnable(GraphInstanceNode runtimeNode);
	internal abstract void OnDisable(GraphInstanceNode runtimeNode);
	internal abstract void OnInputChanged(GraphInstanceNode runtimeNode);
}

public abstract class NodeRuntime<TNode> : NodeRuntime
	where TNode : Node
{
	public virtual void OnEnable(GraphInstanceNode<TNode> runtimeNode)
	{
	}

	public virtual void OnDisable(GraphInstanceNode<TNode> runtimeNode)
	{
	}

	public virtual void OnInputChanged(GraphInstanceNode<TNode> runtimeNode)
	{
	}

	internal override void OnEnable(GraphInstanceNode runtimeNode)
	{
		OnEnable(new GraphInstanceNode<TNode>(runtimeNode.GraphRuntime, (TNode)runtimeNode.Node));
	}

	internal override void OnDisable(GraphInstanceNode runtimeNode)
	{
		OnDisable(new GraphInstanceNode<TNode>(runtimeNode.GraphRuntime, (TNode)runtimeNode.Node));
	}

	internal override void OnInputChanged(GraphInstanceNode runtimeNode)
	{
		OnInputChanged(new GraphInstanceNode<TNode>(runtimeNode.GraphRuntime, (TNode)runtimeNode.Node));
	}
}
