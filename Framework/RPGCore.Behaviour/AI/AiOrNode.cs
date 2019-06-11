using System;

namespace RPGCore.Behaviour
{
	public class AiOrNode : Node<AiOrNode.Metadata>
	{
		public InputSocket RequirementA;
		public InputSocket RequirementB;
		public OutputSocket Selected = new OutputSocket ();

		public override InputMap[] Inputs (IGraphInstance graph, Metadata instance) => new[]
		{
			graph.Connect(ref RequirementA, ref instance.requirementA),
			graph.Connect(ref RequirementB, ref instance.requirementB)
		};

		public override OutputMap[] Outputs (IGraphInstance graph, Metadata instance) => null;

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
					return new Random ().Next (0, 2) == 1 ? requirementA : requirementB;
				}
			}

			public void Setup (IGraphInstance graph, Node parent, Actor target)
			{
				this.target = target;
				AiOrNode stats = (AiOrNode)parent;

				graph.Connect (ref stats.RequirementA, ref requirementA);
				graph.Connect (ref stats.RequirementB, ref requirementB);

				Console.ForegroundColor = ConsoleColor.Gray;
				//Console.WriteLine("AiOrNode: Fetching A " + requirementA.Weight);
				//Console.WriteLine("AiOrNode: Fetching B " + requirementB.Weight);
			}

			public void Remove ()
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine ("AiOrNode: Removed Behaviour on " + target);
			}
		}
	}
}
