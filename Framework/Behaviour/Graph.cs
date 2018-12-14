namespace Behaviour
{
	public class Graph
	{
		public readonly Node[] Nodes;

		public int OutputCount;

		public Graph(Node[] nodes)
		{
			Nodes = nodes;


			INodeInstance[] behaviourTokens = new INodeInstance[Nodes.Length];

			int nodeCount = Nodes.Length;

			// Map and create tokens
			for (int i = 0; i < nodeCount; i++)
			{
				var node = Nodes[i];
				behaviourTokens[i] = node.Create();
			}

			var graphInstance = new GraphInstance(this, behaviourTokens);

			// Connect the token sockets
			for (int i = 0; i < nodeCount; i++)
			{
				var conns = Nodes[i].ConnectToken(graphInstance, behaviourTokens[i]);

				for (int j = 0; j < conns.Length; j++)
				{
					var conn = conns[j];
					
					//if 9
				}
			}


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
			INodeInstance[] behaviourTokens = new INodeInstance[Nodes.Length];

			int nodeCount = Nodes.Length;

			// Map and create tokens
			for (int i = 0; i < nodeCount; i++)
			{
				var node = Nodes[i];
				behaviourTokens[i] = node.Create();
			}

			var graphInstance = new GraphInstance(this, behaviourTokens);

			// Connect the token sockets
			for (int i = 0; i < nodeCount; i++)
			{
				Nodes[i].ConnectToken(graphInstance, behaviourTokens[i]);
			}

			// Setup tokens
			for (int i = 0; i < nodeCount; i++)
			{
				Nodes[i].Setup(graphInstance, behaviourTokens[i], target);
			}

			return graphInstance;
		}
	}
}
