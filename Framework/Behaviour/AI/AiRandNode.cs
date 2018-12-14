using System;

namespace Behaviour
{
	public class AiRandNode : Node<AiRandNode.Metadata>
	{
		public OutputSocket Rand = new OutputSocket();

		public override SocketMap[] Connect(GraphInstance graph, Metadata instance) => new[]
		{
			graph.Connect(ref Rand, out instance.random)
		};

		public class Metadata : IAiNode
		{
			public ILazyOutput<int> random;

			private Actor target;

			public int LocalWeight => new Random().Next(10, 100);
			public IAiNode Source => null;

			public void Setup(GraphInstance graph, Node parent, Actor target)
			{
				this.target = target;
				AiRandNode stats = (AiRandNode)parent;

				random.OnRequested += RandomRequested;

				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("AiRandNode: Setup Behaviour on " + this.target);
			}

			public void RandomRequested()
			{
				int value = new Random().Next(0, 20);
				random.Value = value;
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.WriteLine("AiRandNode: Random Outputted " + value);
			}

			public void Remove()
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("AiRandNode: Removed Behaviour on " + target);
			}
		}
	}
}
