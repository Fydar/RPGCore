namespace RPGCore.Behaviour;

public abstract class NodeRuntime
{
	internal abstract void OnEnable(RuntimeNode node);
	internal abstract void OnDisable(RuntimeNode node);
	internal abstract void OnInputChanged(RuntimeNode node);
}

public abstract class NodeRuntime<TNode> : NodeRuntime
	where TNode : Node
{
	public virtual void OnEnable(RuntimeNode<TNode> node)
	{
	}

	public virtual void OnDisable(RuntimeNode<TNode> node)
	{
	}

	public virtual void OnInputChanged(RuntimeNode<TNode> node)
	{
	}

	internal override void OnEnable(RuntimeNode node)
	{
		OnEnable(new RuntimeNode<TNode>(node.GraphRuntime, (TNode)node.Node));
	}

	internal override void OnDisable(RuntimeNode node)
	{
		OnDisable(new RuntimeNode<TNode>(node.GraphRuntime, (TNode)node.Node));
	}

	internal override void OnInputChanged(RuntimeNode node)
	{
		OnInputChanged(new RuntimeNode<TNode>(node.GraphRuntime, (TNode)node.Node));
	}
}
