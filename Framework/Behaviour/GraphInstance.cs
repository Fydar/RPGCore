namespace Behaviour
{
	public class GraphInstance : IBehaviour
	{
		private readonly INodeInstance[] removeNodes;

		public readonly object[] connections;

		private bool canCreate = false;

		public GraphInstance(Graph graph)
		{
			int nodeCount = graph.Nodes.Length;
			INodeInstance[] behaviourTokens = new INodeInstance[nodeCount];

			// Map and create tokens
			for (int i = 0; i < nodeCount; i++)
			{
				var node = graph.Nodes[i];
				behaviourTokens[i] = node.Create();
			}
			this.removeNodes = behaviourTokens;

			connections = new object[graph.OutputCount];

			// Allow all used inputs to setup their connections.
			canCreate = true;
			for (int i = 0; i < nodeCount; i++)
			{
				graph.Nodes[i].Inputs(this, behaviourTokens[i]);
			}

			// Allow all outputs to assign themselves to that connection
			canCreate = false;
			for (int i = 0; i < nodeCount; i++)
			{
				graph.Nodes[i].Outputs(this, behaviourTokens[i]);
			}
		}

		public void Setup(Actor target)
		{
			// Setup tokens
			for (int i = 0; i < nodeCount; i++)
			{
				graph.Nodes[i].Setup(this, behaviourTokens[i], target);
			}
		}

		public void Remove()
		{
			foreach (var node in removeNodes)
			{
				node.Remove();
			}
		}

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

		public InputMap Connect<T>(ref InputSocket socket, out IInput<T> connection)
		{
			connection = Connect<T>(socket.TargetId);
			return new InputMap(socket, connection, typeof(T));
		}

		public OutputMap Connect<T>(ref OutputSocket socket, out IOutput<T> connection)
		{
			connection = Connect<T>(socket.Id);
			return new OutputMap(socket, connection);
		}

		public OutputMap Connect<T>(ref OutputSocket socket, out ILazyOutput<T> connection)
		{
			connection = Connect<T>(socket.Id);
			return new OutputMap(socket, connection);
		}

		public InputMap Connect<T>(ref InputSocket socket, out T connection)
			where T : INodeInstance
		{
			connection = (T)removeNodes[socket.TargetId];
			return new InputMap(socket, connection);
		}

		public void Connect(ref InputSocket input, out INodeInstance connection)
		{
			connection = removeNodes[input.TargetId];
		}

		private RequestingConnection<T> LazyConnect<T>(int id)
		{
			if (id == -1)
				return null;

			object shared = connections[id];
			if (canCreate && shared == null)
			{
				shared = new RequestingConnection<T>();
				connections[id] = shared;
			}
			return (RequestingConnection<T>)shared;
		}

		private Connection<T> Connect<T>(int id)
		{
			if (id == -1)
				return null;
			
			object shared = connections[id];
			if (canCreate && shared == null)
			{
				shared = new Connection<T>();
				connections[id] = shared;
			}
			return (Connection<T>)shared;
		}
	}
}
