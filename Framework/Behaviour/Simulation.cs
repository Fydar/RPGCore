using Behaviour.Packages;
using System;
using System.Threading;

namespace Behaviour
{
	public class Simulation
	{
		public Graph LongswordItem
		{
			get
			{
				var rollA = new RollNode { MinValue = 20, MaxValue = 40 };
				var rollB = new RollNode { MaxValue = 12 };

				var stat = new StatsNode
				{
					ValueA = new InputSocket(0),
					ValueB = new InputSocket(1)
				};

				return new Graph(new Node[]
				{
					stat,
					rollA,
					rollB,
				});
			}
		}

		public void Start()
		{
			Console.WriteLine ("Outputting Node Manifest...");
			Console.WriteLine(Manifest.NodeManifest.Construct (new Type[] { typeof(StatsNode), typeof(RollNode) }));

			//Console.WriteLine ("Outputting Type Manifest...");
			//Console.WriteLine(Manifest.TypeManifest.ConstructBaseTypes ());


			

			Console.WriteLine ("Running Simulation...");

			var player = new Actor();

			IBehaviour longswordEquipt = LongswordItem.Setup(player);
			for (int i = 0; i < 5; i++)
			{
				Thread.Sleep(100);
				player.Health.Value -= 20;
			}
			longswordEquipt.Remove();

			/*
			var rollA = new AiRandNode();
			var rollB = new AiRandNode();
			var rollC = new AiRandNode();

			var orNode = new AiOrNode
			{
				RequirementA = new InputSocket(0),
				RequirementB = new InputSocket(1)
			};

			var outputNode = new AiOrNode
			{
				RequirementA = new InputSocket(2),
				RequirementB = new InputSocket(3)
			};

			var UnitAI = new Graph(new Node[]
			{
				rollA,
				rollB,
				orNode,
				rollC,
				outputNode
			});
			IBehaviourToken aiInstance = UnitAI.Setup(player);
			var output = (AiOrNode.Metadata)aiInstance.GetNode<AiOrNode.Metadata>();
			

			for (int i = 0; i < 5; i++)
			{
				Thread.Sleep(100);
				IAiNode current = output;
				string indent = "";
				do
				{
					current = current.Source;
					if (current != null)
						Console.WriteLine(indent + current.GetHashCode() + " - " + current);
					indent += "  ";

				} while (current.Source != null);
			}*/

			Thread.Sleep(100);

			// aiInstance.Remove();

			for (int i = 0; i < 5; i++)
			{
				Thread.Sleep(100);
				player.Health.Value -= 20;
			}
		}
	}
}