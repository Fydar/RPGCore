using Newtonsoft.Json;
using RPGCore.Behaviour.Manifest;
using RPGCore.Behaviour.Packages;
using System;
using System.IO;
using System.Threading;

namespace RPGCore.Behaviour
{
	public class Simulation
	{
		public void Start ()
		{
			IntEventField left = new IntEventField (1);
			IntEventField right = new IntEventField (3);

			left += (int newValue) =>
			{
				Console.WriteLine ("New Value: " + newValue);
			};

			left.Value += 5;

			Console.WriteLine ($"{left.Value} + {right.Value} = " + (left + right).Calculate ());

			IntEventField one = new IntEventField (1);

			IntEventField test = new IntEventField (left + right + one);

			left.Value++;
			right.Value++;

			Console.WriteLine ($"{left.Value} + {right.Value} + {one.Value} = " + test.Value);


			var nodes = NodeManifest.Construct (new Type[] { typeof (StatsNode), typeof (RollNode) });
			var types = TypeManifest.ConstructBaseTypes ();

			var manifest = new BehaviourManifest ()
			{
				Nodes = nodes,
				Types = types
			};

			File.WriteAllText ("Content/RPGCoreMath.bmfst", manifest.ToString ());

			Console.WriteLine ("Importing Graph...");

			var pkg = PackageExplorer.Load ("Content/Core");
			Console.WriteLine (pkg.Name);
			pkg.WritePackage ("Content/Core.bpkg");

			var packageItem = JsonConvert.DeserializeObject<PackageItem> (File.ReadAllText ("Content/Core/Longsword/Main.bhvr"));
			Console.WriteLine ("\tImported: " + packageItem.Name);
			var unpackedGraph = packageItem.Unpack ();

			Console.WriteLine ("Running Simulation...");

			var player = new Actor ();

			IBehaviour instancedItem = unpackedGraph.Setup (player);
			for (int i = 0; i < 5; i++)
			{
				Thread.Sleep (100);
				player.Health.Value -= 20;
			}
			instancedItem.Remove ();

			for (int i = 0; i < 5; i++)
			{
				Thread.Sleep (100);
				player.Health.Value -= 20;
			}
		}
	}
}