using System.Collections.Generic;
using System.Linq;

namespace Behaviour
{
	public class Graph
	{
		public readonly Dictionary<string, Node> NodesMap;
		public readonly Node[] Nodes;
		public readonly int OutputCount;

		public Graph(Dictionary<string, Node> nodesMap, int outputCount)
		{
			NodesMap = nodesMap;
			Nodes = NodesMap.Values.ToArray();
			OutputCount = outputCount;
		}

		public IBehaviour Setup(Actor target)
		{
			var graph = new GraphInstance(this);
			graph.Setup(target);
			return graph;
		}
	}
}
