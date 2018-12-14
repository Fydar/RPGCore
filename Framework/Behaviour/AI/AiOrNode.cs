using System;

namespace Behaviour
{
	public class AiOrNode : Node<AiOrNode.Metadata>
	{
		public InputSocket RequirementA;
		public InputSocket RequirementB;
		public OutputSocket Selected = new OutputSocket();

		public override SocketMap[] Connect(GraphInstance graph, Metadata instance) => new[]
		{
			graph.Connect(ref RequirementA, out instance.requirementA),
			graph.Connect(ref RequirementB, out instance.requirementB)
		};

		public class Metadata : IAiNode
		{
			public IAiNode requirementA;
			public IAiNode requirementB;

			private Actor target;

			public int LocalWeight
			{
				get
				{
					return 0;
				}
			}

			public IAiNode Source
			{
				get
				{
					return new Random().Next(0, 2) == 1 ? requirementA : requirementB;
				}
			}

			public void Setup(GraphInstance graph, Node parent, Actor target)
			{
				this.target = target;
				AiOrNode stats = (AiOrNode)parent;

				graph.Connect(ref stats.RequirementA, out requirementA);
				graph.Connect(ref stats.RequirementB, out requirementB);

				Console.ForegroundColor = ConsoleColor.Gray;
				//Console.WriteLine("AiOrNode: Fetching A " + requirementA.Weight);
				//Console.WriteLine("AiOrNode: Fetching B " + requirementB.Weight);
			}

			public void Remove()
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("AiOrNode: Removed Behaviour on " + target);
			}
		}
	}
}
