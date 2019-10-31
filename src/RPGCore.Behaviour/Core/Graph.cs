using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace RPGCore.Behaviour
{
	public sealed class Graph
	{
		public string Name;
		public readonly Node[] Nodes;
		public readonly int ConnectionsCount;
		public Dictionary<string, Graph> SubGraphs;

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

		public Graph (Node[] nodes, int connectionCount, Dictionary<string, Graph> subGraphs)
		{
			Nodes = nodes;
			ConnectionsCount = connectionCount;
			SubGraphs = subGraphs;
		}

		public GraphInstance Create (IDictionary<LocalId, JObject> data = null)
		{
			return new GraphInstance (this, data);
		}
	}
}
