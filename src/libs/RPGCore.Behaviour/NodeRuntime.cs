namespace RPGCore.Behaviour;

public abstract class NodeRuntime
{
	internal abstract void OnEnable(RuntimeNode runtimeNode);
	internal abstract void OnDisable(RuntimeNode runtimeNode);
	internal abstract void OnInputChanged(RuntimeNode runtimeNode);
}

public abstract class NodeRuntime<TNode> : NodeRuntime
	where TNode : Node
{
	public virtual void OnEnable(RuntimeNode<TNode> runtimeNode)
	{
	}

	public virtual void OnDisable(RuntimeNode<TNode> runtimeNode)
	{
	}

	public virtual void OnInputChanged(RuntimeNode<TNode> runtimeNode)
	{
	}

	internal override void OnEnable(RuntimeNode runtimeNode)
	{
		OnEnable(new RuntimeNode<TNode>(runtimeNode.GraphRuntime, (TNode)runtimeNode.Node));
	}

	internal override void OnDisable(RuntimeNode runtimeNode)
	{
		OnDisable(new RuntimeNode<TNode>(runtimeNode.GraphRuntime, (TNode)runtimeNode.Node));
	}

	internal override void OnInputChanged(RuntimeNode runtimeNode)
	{
		OnInputChanged(new RuntimeNode<TNode>(runtimeNode.GraphRuntime, (TNode)runtimeNode.Node));
	}
}
