using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace RPGCore.Behaviour
{
	public sealed class GraphInstance : IGraphInstance, IGraphConnections
	{
		private readonly Graph graph;
		private readonly INodeInstance[] nodeInstances;
		private readonly IConnection[] connections;
		private readonly InputMap[][] allInputs;
		private readonly OutputMap[][] allOutputs;

		private INodeInstance currentNode;

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

			var serializer = new JsonSerializer ();
			serializer.Converters.Add (new OutputConverter ());

			// Map and create tokens
			for (int i = 0; i < nodeCount; i++)
			{
				var node = graph.Nodes[i];

				var instance = node.CreateInstance ();

				if (data != null && data.TryGetValue (node.Id, out var instanceData))
				{
					using (var sr = instanceData.CreateReader ())
					{
						serializer.Populate (sr, instance);
					}
				}

				nodeInstances[i] = instance;
			}

			connections = new IConnection[graph.ConnectionsCount];

			// Allow all outputs to make their output type visible
			allOutputs = new OutputMap[nodeCount][];
			for (int i = 0; i < nodeCount; i++)
			{
				currentNode = nodeInstances[i];
				allOutputs[i] = graph.Nodes[i].Outputs (this, currentNode);
			}

			// Allow inputs to subscribe to those outputs.
			allInputs = new InputMap[nodeCount][];
			for (int i = 0; i < nodeCount; i++)
			{
				currentNode = nodeInstances[i];
				allInputs[i] = graph.Nodes[i].Inputs (this, currentNode);
			}

			// Stop events from taking effect immediately.
			foreach (var connection in connections)
			{
				connection.BufferEvents = true;
			}
		}

		public void Setup ()
		{
			int nodeCount = graph.Nodes.Length;

			// Start allowing events to fire
			for (int i = 0; i < nodeCount; i++)
			{
				var nodeInstance = nodeInstances[i];
				var nodeInputs = allInputs[i];

				if (nodeInputs == null)
				{
					continue;
				}

				foreach (var input in nodeInputs)
				{
					connections[input.ConnectionId].RegisterInput (nodeInstance);
				}
			}

			// Run node setup process
			for (int i = 0; i < nodeCount; i++)
			{
				currentNode = nodeInstances[i];
				graph.Nodes[i].Setup (this, currentNode);
			}

			// Release all buffered events.
			foreach (var connection in connections)
			{
				connection.BufferEvents = false;
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

		InputMap IGraphConnections.Connect<T> (ref InputSocket socket, ref Input<T> input)
		{
			if (socket.ConnectionId >= 0)
			{
				var connection = GetConnection (socket.ConnectionId);

				if (connection.ConnectionType == typeof (int)
					&& typeof (T) == typeof (float))
				{
					var converter = new IntToFloatConverter ();
					converter.SetSource (connection);
					connection.RegisterConverter (converter);

					input = new Input<T> (currentNode, converter);
				}
				else
				{
					input = new Input<T> (currentNode, connection);
				}
			}

			return new InputMap (socket, typeof (T));
		}

		OutputMap IGraphConnections.Connect<T> (ref OutputSocket socket, ref Output<T> output)
		{
			var newConnection = GetOrCreateConnection<T> (socket.Id);
			if (newConnection != null && output.Connection != null)
			{
				newConnection.Value = output.Connection.Value;
			}
			output = new Output<T> (newConnection);

			return new OutputMap (socket, typeof (T));
		}

		InputMap IGraphConnections.Connect<T> (ref InputSocket socket, ref T node)
		{
			if (socket.ConnectionId >= 0)
			{
				node = (T)nodeInstances[socket.ConnectionId];
			}

			return new InputMap (socket, typeof (T));
		}

		private IConnection<T> GetOrCreateConnection<T> (int id)
		{
			if (id < 0)
			{
				return null;
			}

			var shared = connections[id];
			if (shared == null)
			{
				shared = new BasicConnection<T> (id);
				connections[id] = shared;
			}
			return (IConnection<T>)shared;
		}

		private IConnection GetConnection (int id)
		{
			return id < 0
				? null
				: connections[id];
		}

		public InputSource GetSource<T> (Input<T> input)
		{
			if (input.Connection == null)
			{
				return new InputSource ();
			}

			int connectionId = input.Connection.ConnectionId;

			if (connectionId == -1)
			{
				return new InputSource ();
			}

			for (int x = 0; x < allOutputs.Length; x++)
			{
				var outputSet = allOutputs[x];

				if (outputSet == null)
				{
					continue;
				}

				for (int y = 0; y < outputSet.Length; y++)
				{
					var output = outputSet[y];

					if (output.ConnectionId == connectionId)
					{
						var instance = nodeInstances[x];
						var node = graph.Nodes[x];
						return new InputSource (node, instance, output);
					}
				}
			}
			return new InputSource ();
		}

		public IEnumerable<OutputSource> GetSource<T> (Output<T> output)
		{
			if (output.Connection == null)
			{
				yield break;
			}

			int connectionId = output.Connection.ConnectionId;

			if (connectionId == -1)
			{
				yield break;
			}

			for (int x = 0; x < allInputs.Length; x++)
			{
				var inputSet = allInputs[x];

				if (inputSet == null)
				{
					continue;
				}

				for (int y = 0; y < inputSet.Length; y++)
				{
					var input = inputSet[y];

					if (input.ConnectionId == connectionId)
					{
						var instance = nodeInstances[x];
						yield return new OutputSource (instance, input);
					}
				}
			}
		}

		public T GetNodeInstance<T> ()
			where T : INodeInstance
		{
			for (int i = 0; i < nodeInstances.Length; i++)
			{
				var instance = nodeInstances[i];

				if (typeof (T).IsAssignableFrom (instance.GetType ()))
				{
					return (T)instance;
				}
			}
			return default;
		}

		public IEnumerable<T> GetNodeInstances<T> ()
			where T : INodeInstance
		{
			for (int i = 0; i < nodeInstances.Length; i++)
			{
				var instance = nodeInstances[i];

				if (typeof (T).IsAssignableFrom (instance.GetType ()))
				{
					yield return (T)instance;
				}
			}
		}

		public void SetInput<T> (T input)
		{
			foreach (var node in GetNodeInstances<IInputNode<T>> ())
			{
				node.OnReceiveInput (input);
			}
		}
	}
}
