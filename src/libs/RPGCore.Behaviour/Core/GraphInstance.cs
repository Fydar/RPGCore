using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Behaviour
{
	public sealed class GraphInstance : IGraphInstance, IConnectionCallback
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly GraphInstanceNode[] nodes;

		private readonly IConnection[] connections;

		[JsonIgnore]
		public Graph Template { get; }

		public IReadOnlyList<GraphInstanceNode> Nodes => nodes;

		public GraphInstanceNode? this[LocalId id]
		{
			get
			{
				for (int i = 0; i < Template.Nodes.Length; i++)
				{
					var node = Template.Nodes[i];
					if (node.Id == id)
					{
						return nodes[i];
					}
				}
				return null;
			}
		}

		public GraphInstance(Graph template, IDictionary<LocalId, JObject> data = null)
		{
			Template = template;

			int nodeCount = template.Nodes.Length;
			nodes = new GraphInstanceNode[nodeCount];

			var serializer = new JsonSerializer();
			serializer.Converters.Add(new OutputConverter());
			serializer.Converters.Add(new SerializedGraphInstanceProxyConverter(template));

			// Map and create tokens
			for (int i = 0; i < nodeCount; i++)
			{
				var node = template.Nodes[i];

				var instance = node.CreateInstance();

				if (data != null && data.TryGetValue(node.Id, out var instanceData))
				{
					using var sr = instanceData.CreateReader();
					serializer.Populate(sr, instance);
				}

				nodes[i].Instance = instance;
			}

			connections = new IConnection[template.ConnectionsCount];

			// Allow all outputs to make their output type visible
			for (int i = 0; i < nodeCount; i++)
			{
				var currentNode = nodes[i].Instance;
				var connectionMapper = new ConnectionMapper(nodes[i].Instance, this);
				nodes[i].Outputs = template.Nodes[i].Outputs(connectionMapper, currentNode);
			}

			// Allow inputs to subscribe to those outputs.
			for (int i = 0; i < nodeCount; i++)
			{
				var currentNode = nodes[i].Instance;
				var connectionMapper = new ConnectionMapper(currentNode, this);

				nodes[i].Inputs = template.Nodes[i].Inputs(connectionMapper, currentNode);
			}

			// Stop events from taking effect immediately.
			foreach (var connection in connections)
			{
				connection.BufferEvents = true;
			}
		}

		public void Setup()
		{
			int nodeCount = Template.Nodes.Length;

			// Start allowing events to fire
			for (int i = 0; i < nodeCount; i++)
			{
				var nodeInputs = nodes[i].Inputs;
				if (nodeInputs == null)
				{
					continue;
				}

				var nodeInstance = nodes[i].Instance;

				foreach (var input in nodeInputs)
				{
					connections[input.ConnectionId].RegisterInput(nodeInstance);
				}
			}

			// Run node setup process
			for (int i = 0; i < nodeCount; i++)
			{
				var currentNode = nodes[i].Instance;
				Template.Nodes[i].Setup(this, currentNode);
			}

			// Release all buffered events.
			foreach (var connection in connections)
			{
				connection.BufferEvents = false;
			}
		}

		public void Remove()
		{
			foreach (var node in nodes)
			{
				node.Instance.Remove();
			}
		}

		public SerializedGraphInstance Pack()
		{
			var nodeMap = new Dictionary<LocalId, JObject>();

			for (int i = 0; i < nodes.Length; i++)
			{
				var instance = nodes[i].Instance;

				var serializer = new JsonSerializer
				{
					ContractResolver = new IgnoreInputsResolver()
				};
				serializer.Converters.Add(new SerializedGraphInstanceProxyConverter(null));

				nodeMap.Add(instance.Template.Id, JObject.FromObject(instance, serializer));
			}

			return new SerializedGraphInstance()
			{
				NodeInstances = nodeMap
			};
		}

		public InputSource GetSource<T>(Input<T> input)
		{
			if (input.Connection == null)
			{
				return default;
			}

			int connectionId = input.Connection.ConnectionId;

			if (connectionId == -1)
			{
				return default;
			}

			for (int x = 0; x < nodes.Length; x++)
			{
				var node = nodes[x];
				if (node.Outputs == null)
				{
					continue;
				}

				for (int y = 0; y < node.Outputs.Length; y++)
				{
					var output = node.Outputs[y];

					if (output.ConnectionId == connectionId)
					{
						return new InputSource(node.Instance, output);
					}
				}
			}
			return default;
		}

		public IEnumerable<OutputSource> GetSource<T>(Output<T> output)
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

			for (int x = 0; x < nodes.Length; x++)
			{
				var node = nodes[x];

				if (node.Inputs == null)
				{
					continue;
				}

				for (int y = 0; y < node.Inputs.Length; y++)
				{
					var input = node.Inputs[y];

					if (input.ConnectionId == connectionId)
					{
						var instance = node.Instance;
						yield return new OutputSource(instance, input);
					}
				}
			}
		}

		public INodeInstance? GetNode<T>()
		{
			for (int i = nodes.Length - 1; i >= 0; i--)
			{
				var node = nodes[i].Instance;

				if (node.GetType() == typeof(T))
				{
					return node;
				}
			}
			return null;
		}

		public T GetNodeInstance<T>()
			where T : INodeInstance
		{
			for (int i = 0; i < nodes.Length; i++)
			{
				var instance = nodes[i].Instance;

				if (typeof(T).IsAssignableFrom(instance.GetType()))
				{
					return (T)instance;
				}
			}
			return default;
		}

		public IEnumerable<T> GetNodeInstances<T>()
			where T : INodeInstance
		{
			for (int i = 0; i < nodes.Length; i++)
			{
				var instance = nodes[i].Instance;

				if (typeof(T).IsAssignableFrom(instance.GetType()))
				{
					yield return (T)instance;
				}
			}
		}

		public void SetInput<T>(T input)
		{
			foreach (var node in GetNodeInstances<IInputNode<T>>())
			{
				node.OnReceiveInput(input);
			}
		}

		InputMap IConnectionCallback.Connect<T>(INodeInstance parent, ref InputSocket socket, ref Input<T> input)
		{
			if (socket.ConnectionId >= 0)
			{
				var connection = GetConnection(socket.ConnectionId);

				if (connection.ConnectionType == typeof(int)
					&& typeof(T) == typeof(float))
				{
					var converter = connection.UseConverter(typeof(IntToFloatConverter));
					converter.SetSource(connection);

					input = new Input<T>(parent, converter);
				}
				else
				{
					input = new Input<T>(parent, connection);
				}
			}

			return new InputMap(socket, typeof(T));
		}

		OutputMap IConnectionCallback.Connect<T>(INodeInstance parent, ref OutputSocket socket, ref Output<T> output)
		{
			var newConnection = GetOrCreateOutputConnection<T>(socket.Id);
			if (newConnection != null && output.Connection != null)
			{
				newConnection.Value = output.Connection.Value;
			}
			output = new Output<T>(newConnection);

			return new OutputMap(socket, typeof(T));
		}

		InputMap IConnectionCallback.Connect<T>(INodeInstance parent, ref InputSocket socket, ref T node)
		{
			if (socket.ConnectionId >= 0)
			{
				node = (T)nodes[socket.ConnectionId].Instance;
			}

			return new InputMap(socket, typeof(T));
		}

		private IConnection GetConnection(int id)
		{
			return id < 0
				? null
				: connections[id];
		}

		private OutputConnection<T> GetOrCreateOutputConnection<T>(int id)
		{
			if (id < 0)
			{
				return null;
			}

			var shared = (OutputConnection<T>)connections[id];
			if (shared == null)
			{
				shared = new OutputConnection<T>(id);
				connections[id] = shared;
			}
			return shared;
		}
	}
}
