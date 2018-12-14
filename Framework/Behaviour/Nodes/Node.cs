namespace Behaviour
{
	public abstract class Node
	{
		public abstract INodeInstance Create();
		public abstract void Setup(GraphInstance graph, INodeInstance metadata, Actor target);
		public abstract SocketMap[] ConnectToken(GraphInstance graph, object instance);
	}

	public abstract class Node<T> : Node
		where T : INodeInstance, new()
	{
		public override INodeInstance Create()
		{
			return new T();
		}

		public override SocketMap[] ConnectToken(GraphInstance graph, object instance)
		{
			return Connect(graph, (T)instance);
		}

		public abstract SocketMap[] Connect(GraphInstance graph, T instance);

		public override void Setup(GraphInstance graph, INodeInstance metadata, Actor target)
		{
			metadata.Setup(graph, this, target);
		}
	}
}
