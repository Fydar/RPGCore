using Behaviour.Packages;
using Newtonsoft.Json;
using System;
using System.Threading;

namespace Behaviour
{
	public class Simulation
	{
		public void Start()
		{
			BubbleTest.Test();
			Console.WriteLine ("Outputting Node Manifest...");
			Console.WriteLine(Manifest.NodeManifest.Construct (new Type[] { typeof(StatsNode), typeof(RollNode) }));

			//Console.WriteLine ("Outputting Type Manifest...");
			//Console.WriteLine(Manifest.TypeManifest.ConstructBaseTypes ());

			Console.WriteLine("Importing Graph...");
			var packageItem = JsonConvert.DeserializeObject<PackageItem>("{\"Name\":\"BronzeLongsword\",\"Nodes\":{\"0a084c2c\":{\"Type\":\"Behaviour.RollNode\",\"Data\":{\"MinValue\":20,\"MaxValue\":40,\"TooltipFormat\":\"{0}\"},\"_Editor\":{\"Position\":{\"x\":362,\"y\":231}}},\"4a41bc58\":{\"Type\":\"Behaviour.RollNode\",\"Data\":{\"MinValue\":20,\"MaxValue\":40,\"TooltipFormat\":\"{0}\"},\"_Editor\":{\"Position\":{\"x\":362,\"y\":231}}},\"a4f194f\":{\"Type\":\"Behaviour.StatsNode\",\"Data\":{\"ValueA\":\"0a084c2c.Output\",\"ValueB\":\"4a41bc58.Output\"},\"_Editor\":{\"Position\":{\"x\":362,\"y\":231}}}}}");
			Console.WriteLine("Imported: " + packageItem.Name);
			var unpackedGraph = packageItem.Unpack();

			Console.WriteLine ("Running Simulation...");

			var player = new Actor();

			IBehaviour instancedItem = unpackedGraph.Setup(player);
			for (int i = 0; i < 5; i++)
			{
				Thread.Sleep(100);
				player.Health.Value -= 20;
			}
			instancedItem.Remove();

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