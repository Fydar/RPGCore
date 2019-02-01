using Behaviour.Packages;
using Newtonsoft.Json;
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





			Console.WriteLine("Importing Graph...");
			var packageItem = JsonConvert.DeserializeObject<PackageItem>("{\"Name\":\"Bronze Longsword\",\"Nodes\":{\"942388a8\":{\"Type\":\"WeaponInput\",\"Data\":{\"Slot\":\"Mainhand\",\"Stats\":{\"Attack\":10,\"AttackSpeed\":1.55,\"CriticalStrikeChance\":1.5,\"CriticalStrikeMultiplier\":1.5,\"StaminaCost\":20}},\"_Editor\":{\"Position\":{\"x\":53,\"y\":128}}},\"0a084c2c\":{\"Type\":\"GrantStat\",\"Data\":{\"Stat\":\"Attack\",\"Whilst\":\"942388a8.Equipped\",\"Value\":10},\"_Editor\":{\"Position\":{\"x\":362,\"y\":231}}},\"f78014b6\":{\"Type\":\"GrantStat\",\"Data\":{\"Stat\":\"Movement Speed\",\"Whilst\":\"942388a8.Equipped\",\"Value\":10},\"_Editor\":{\"Position\":{\"x\":363,\"y\":84}}},\"5n4a3fc8\":{\"Type\":\"Graph\",\"Data\":{\"Nodes\":{\"4ba187c9\":{\"Type\":\"GrantBuff\",\"Data\":{\"Buff\":\"Burning\",\"Whilst\":\"942388a8.Equipped\",\"Value\":10},\"_Editor\":{\"Position\":{\"x\":190,\"y\":155}}}}},\"_Editor\":{\"Position\":{\"x\":661,\"y\":183}}}}}");
			Console.WriteLine("Imported: " + packageItem.Name);


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