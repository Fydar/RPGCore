using System.Collections.Generic;

namespace RPGCore.Behaviour
{
	public class Graph
	{
		public readonly Node[] Nodes;
		public readonly int OutputCount;

		public Graph (Node[] nodes, int outputCount)
		{
			Nodes = nodes;
			OutputCount = outputCount;
		}

		public IBehaviour Setup (Actor target)
		{
			var graph = new GraphInstance (this);
			graph.Setup (target);
			return graph;
		}
	}
}
