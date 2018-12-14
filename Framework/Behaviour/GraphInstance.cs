namespace Behaviour
{
	public class GraphInstance : IBehaviour
	{
		private readonly INodeInstance[] removeNodes;

		public readonly object[] connections;

		public INodeInstance GetNode<T>()
		{
			for (int i = removeNodes.Length - 1; i >= 0; i--)
			{
				var node = removeNodes[i];

				if (node.GetType() == typeof(T))
					return node;
			}
			return null;
		}

		public SocketMap Connect<T>(ref InputSocket socket, out IInput<T> connection)
		{
			connection = Connect<T>(socket.TargetId);
			return new SocketMap(socket, connection);
		}

		public SocketMap Connect<T>(ref OutputSocket socket, out IOutput<T> connection)
		{
			connection = Connect<T>(socket.Id);
			return new SocketMap(socket, connection);
		}

		public SocketMap Connect<T>(ref OutputSocket socket, out ILazyOutput<T> connection)
		{
			connection = Connect<T>(socket.Id);
			return new SocketMap(socket, connection);
		}

		public SocketMap Connect<T>(ref InputSocket socket, out T connection)
			where T : INodeInstance
		{
			connection = (T)removeNodes[socket.TargetId];
			return new SocketMap(socket, connection);
		}

		public void Connect(ref InputSocket input, out INodeInstance connection)
		{
			connection = removeNodes[input.TargetId];
		}

		private RequestingConnection<T> LazyConnect<T>(int id)
		{
			object shared = connections[id];
			if (shared == null)
			{
				shared = new RequestingConnection<T>();
				connections[id] = shared;
			}
			return (RequestingConnection<T>)shared;
		}

		private Connection<T> Connect<T>(int id)
		{
			object shared = connections[id];
			if (shared == null)
			{
				shared = new Connection<T>();
				connections[id] = shared;
			}
			return (Connection<T>)shared;
		}

		public GraphInstance(Graph graph, INodeInstance[] removeNodes)
		{
			this.removeNodes = removeNodes;

			connections = new object[graph.OutputCount];
		}

		public void Remove()
		{
			foreach (var node in removeNodes)
			{
				node.Remove();
			}
		}
	}
}
