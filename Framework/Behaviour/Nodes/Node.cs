namespace Behaviour
{
	public abstract class Node
	{
		public abstract INodeInstance Create();
		public abstract void Setup(GraphInstance graph, INodeInstance metadata, Actor target);

		public abstract InputMap[] Inputs(GraphInstance graph, object instance);
		public abstract OutputMap[] Outputs(GraphInstance graph, object instance);
	}

	public abstract class Node<T> : Node
		where T : INodeInstance, new()
	{
		public abstract InputMap[] Inputs(GraphInstance graph, T instance);
		public abstract OutputMap[] Outputs(GraphInstance graph, T instance);

		public override INodeInstance Create()
		{
			return new T();
		}

		public sealed override InputMap[] Inputs(GraphInstance graph, object instance)
		{
			return Inputs(graph, (T)instance);
		}

		public sealed override OutputMap[] Outputs(GraphInstance graph, object instance)
		{
			return Outputs(graph, (T)instance);
		}

		public sealed override void Setup(GraphInstance graph, INodeInstance metadata, Actor target)
		{
			metadata.Setup(graph, this, target);
		}
	}
}
