using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace RPGCore.Behaviour
{
	public sealed class Graph
	{
		public string Name;
		public readonly Node[] Nodes;
		public readonly int ConnectionsCount;

		public Node this[LocalId id]
		{
			get
			{
				foreach (var node in Nodes)
				{
					if (node.Id == id)
					{
						return node;
					}
				}
				return null;
			}
		}

		public Graph (Node[] nodes, int connectionCount)
		{
			Nodes = nodes;
			ConnectionsCount = connectionCount;
		}

		public GraphInstance Setup (IDictionary<LocalId, JObject> data = null)
		{
			var graph = Create (data);
			graph.Setup ();
			return graph;
		}

		public GraphInstance Create (IDictionary<LocalId, JObject> data = null)
		{
			return new GraphInstance (this, data);
		}
	}
}
