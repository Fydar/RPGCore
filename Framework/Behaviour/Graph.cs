namespace Behaviour
{
	public class Graph
	{
		public readonly Node[] Nodes;

		public int OutputCount;

		public Graph(Node[] nodes)
		{
			Nodes = nodes;

			int nodeCount = Nodes.Length;

			for (int i = 0, NodesLength = Nodes.Length; i < NodesLength; i++)
			{
				var node = Nodes[i];

				if (node.GetType() == typeof(StatsNode))
				{
					var statsNode = (StatsNode)node;

				}
				else if (node.GetType() == typeof(RollNode))
				{
					var rollNode = (RollNode)node;

					rollNode.Output.Id = OutputCount;
					OutputCount++;
				}
				else if (node.GetType() == typeof(AiRandNode))
				{
					var rollNode = (AiRandNode)node;

					rollNode.Rand.Id = OutputCount;
					OutputCount++;
				}
				else if (node.GetType() == typeof(AiOrNode))
				{
					var rollNode = (AiOrNode)node;

					rollNode.Selected.Id = OutputCount;
					OutputCount++;
				}
			}
		}

		public IBehaviour Setup(Actor target)
		{
			var graph = new GraphInstance(this);
			graph.Setup(target);
			return graph;
		}
	}
}
