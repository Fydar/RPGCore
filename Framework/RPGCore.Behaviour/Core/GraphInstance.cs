using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace RPGCore.Behaviour
{
	public class GraphInstance : IGraphInstance, IGraphConnections
	{
		private readonly Graph graph;
		private readonly INodeInstance[] nodeInstances;
		private readonly Connection[] connections;
		private readonly InputMap[][] allInputs;
		private readonly OutputMap[][] allOutputs;

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

			connections = new Connection[graph.OutputCount];

			// Allow all used inputs to setup their connections.
			allInputs = new InputMap[nodeCount][];
			for (int i = 0; i < nodeCount; i++)
			{
				allInputs[i] = graph.Nodes[i].Inputs (this, nodeInstances[i]);
			}

			// Allow all outputs to assign themselves to that connection
			allOutputs = new OutputMap[nodeCount][];
			for (int i = 0; i < nodeCount; i++)
			{
				allOutputs[i] = graph.Nodes[i].Outputs (this, nodeInstances[i]);
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

		InputMap IGraphConnections.Connect<T> (ref InputSocket socket, ref Input<T> connection)
		{
			if (socket.TargetId > 0)
			{
				connection = new Input<T>(GetOrCreateConnection<T> (socket.TargetId));
			}

			return new InputMap (socket, typeof (T));
		}

		OutputMap IGraphConnections.Connect<T> (ref OutputSocket socket, ref Output<T> output)
		{
			var newConnection = GetConnection<T> (socket.Id);
			if (newConnection != null && output.Connection != null)
			{
				newConnection.Value = output.Connection.Value;
			}
			output = new Output<T>(newConnection);

			return new OutputMap (socket, typeof (T));
		}

		InputMap IGraphConnections.Connect<T> (ref InputSocket socket, ref T connection)
		{
			if (socket.TargetId > 0)
			{
				connection = (T)nodeInstances[socket.TargetId];
			}

			return new InputMap (socket, typeof (T));
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
