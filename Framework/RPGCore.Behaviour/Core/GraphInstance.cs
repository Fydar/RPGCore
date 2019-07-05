using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace RPGCore.Behaviour
{
	public class GraphInstance : IGraphInstance
	{
		private readonly Graph graph;
		private readonly INodeInstance[] nodeInstances;
		private readonly Connection[] connections;

		public INodeInstance this[LocalId id]
		{
			get
			{
				for (int i = 0; i < graph.Nodes.Length; i++)
				{
					var node = graph.Nodes[i];
					if (node.Id == id)
					{
						return nodeInstances[i];
					}
				}
				return null;
			}
		}

		public GraphInstance (Graph graph, IDictionary<LocalId, JObject> data = null)
		{
			this.graph = graph;

			int nodeCount = graph.Nodes.Length;
			nodeInstances = new INodeInstance[nodeCount];
			connections = new Connection[graph.OutputCount];

			// Map and create tokens
			for (int i = 0; i < nodeCount; i++)
			{
				var node = graph.Nodes[i];

				if (data != null && data.TryGetValue (node.Id, out var instanceData))
				{
					var serializer = new JsonSerializer ();
					serializer.Converters.Add (new OutputConverter ());

					nodeInstances[i] = (INodeInstance)instanceData.ToObject (node.MetadataType, serializer);
				}
				else
				{
					nodeInstances[i] = node.Create ();
				}
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

		public SerializedGraphInstance Pack ()
		{
			var nodeMap = new Dictionary<LocalId, JObject> ();

			for (int i = 0; i < nodeInstances.Length; i++)
			{
				var instance = nodeInstances[i];
				var node = graph.Nodes[i];

				var serializer = new JsonSerializer
				{
					ContractResolver = new IgnoreInputsResolver ()
				};

				nodeMap.Add (node.Id, JObject.FromObject (instance, serializer));
			}

			return new SerializedGraphInstance ()
			{
				GraphType = graph.Name,
				NodeInstances = nodeMap
			};
		}

		public INodeInstance GetNode<T> ()
		{
			for (int i = nodeInstances.Length - 1; i >= 0; i--)
			{
				var node = nodeInstances[i];

				if (node.GetType () == typeof (T))
				{
					return node;
				}
			}
			return null;
		}

		public InputMap Connect<T> (ref InputSocket socket, ref IInput<T> connection)
		{
			if (socket.TargetId > 0)
			{
				connection = GetOrCreateConnection<T> (socket.TargetId);
			}

			return new InputMap (socket, typeof (T), connection);
		}

		public OutputMap Connect<T> (ref OutputSocket socket, ref IOutput<T> connection)
		{
			var newConnection = GetConnection<T> (socket.Id);
			if (connection != null)
			{
				newConnection.Value = ((Connection<T>)connection).Value;
			}
			connection = newConnection;

			return new OutputMap (socket, typeof (T), newConnection);
		}

		public OutputMap Connect<T> (ref OutputSocket socket, ref ILazyOutput<T> connection)
		{
			var newConnection = GetConnection<T> (socket.Id);
			if (connection != null)
			{
				newConnection.Value = ((Connection<T>)connection).Value;
			}
			connection = newConnection;
			return new OutputMap (socket, typeof (T), newConnection);
		}

		public InputMap Connect<T> (ref InputSocket socket, ref T connection)
			where T : INodeInstance
		{
			if (socket.TargetId > 0)
			{
				connection = (T)nodeInstances[socket.TargetId];
			}

			return new InputMap (socket, typeof (T), connection);
		}

		private Connection<T> GetOrCreateConnection<T> (int id)
		{
			if (id < 0)
			{
				return null;
			}

			var shared = connections[id];
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
			{
				return null;
			}

			var shared = connections[id];
			return (Connection<T>)shared;
		}
	}
}
