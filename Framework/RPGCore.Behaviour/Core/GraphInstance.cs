namespace RPGCore.Behaviour
{
	public class GraphInstance : IGraphInstance
	{
		private readonly Graph graph;
		private Node[] nodes;
		private readonly INodeInstance[] nodeInstances;
		private readonly object[] connections;

		public GraphInstance (Graph graph)
		{
			this.graph = graph;
			nodes = graph.Nodes;
			int nodeCount = graph.Nodes.Length;
			nodeInstances = new INodeInstance[nodeCount];
			connections = new object[graph.OutputCount];

			// Map and create tokens
			for (int i = 0; i < nodeCount; i++)
			{
				var node = graph.Nodes[i];
				nodeInstances[i] = node.Create ();
			}

			// Allow all used inputs to setup their connections.
			for (int i = 0; i < nodeCount; i++)
			{
				graph.Nodes[i].Inputs (this, nodeInstances[i]);
			}

			// Allow all outputs to assign themselves to that connection
			for (int i = 0; i < nodeCount; i++)
			{
				graph.Nodes[i].Outputs (this, nodeInstances[i]);
			}
		}

		public void Setup (Actor target)
		{
			int nodeCount = graph.Nodes.Length;
			for (int i = 0; i < nodeCount; i++)
			{
				graph.Nodes[i].Setup (this, nodeInstances[i], target);
			}
		}

		public void Remove ()
		{
			foreach (var node in nodeInstances)
			{
				node.Remove ();
			}
		}

		public INodeInstance GetNode<T> ()
		{
			for (int i = nodeInstances.Length - 1; i >= 0; i--)
			{
				var node = nodeInstances[i];

				if (node.GetType () == typeof (T))
					return node;
			}
			return null;
		}

		public InputMap Connect<T> (ref InputSocket socket, out IInput<T> connection)
		{
			connection = GetOrCreateConnection<T> (socket.TargetId);

			return new InputMap (socket, typeof (T), connection);
		}

		public OutputMap Connect<T> (ref OutputSocket socket, out IOutput<T> connection)
		{
			connection = GetConnection<T> (socket.Id);
			return new OutputMap (socket, typeof (T), connection);
		}

		public OutputMap Connect<T> (ref OutputSocket socket, out ILazyOutput<T> connection)
		{
			connection = GetConnection<T> (socket.Id);
			return new OutputMap (socket, typeof (T), connection);
		}

		public InputMap Connect<T> (ref InputSocket socket, out T connection)
			where T : INodeInstance
		{
			connection = (T)nodeInstances[socket.TargetId];
			return new InputMap (socket, typeof (T), connection);
		}

		public void Connect (ref InputSocket input, out INodeInstance connection)
		{
			connection = nodeInstances[input.TargetId];
		}

		private Connection<T> GetOrCreateConnection<T> (int id)
		{
			if (id < 0)
				return null;

			object shared = connections[id];
			if (shared == null)
			{
				shared = new Connection<T> ();
				connections[id] = shared;
			}
			return (Connection<T>)shared;
		}

		private Connection<T> GetConnection<T> (int id)
		{
			if (id < 0)
				return null;

			object shared = connections[id];
			return (Connection<T>)shared;
		}
	}
}
