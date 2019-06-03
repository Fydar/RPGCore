using Newtonsoft.Json;
using RPGCore.Behaviour.Manifest;
using RPGCore.Packages;
using System;
using System.IO;
using System.Text;
using System.Threading;

namespace RPGCore.Behaviour
{
	public class Simulation
	{
		public void Start ()
		{
			var nodes = NodeManifest.Construct (new Type[] { typeof (StatsNode), typeof (RollNode) });
			var types = TypeManifest.ConstructBaseTypes ();

			var manifest = new BehaviourManifest ()
			{
				Nodes = nodes,
				Types = types
			};

			File.WriteAllText ("Content/RPGCoreMath.bmfst", manifest.ToString ());

			Console.WriteLine ("Importing Graph...");

			var proj = ProjectExplorer.Load ("Content/Tutorial");
			Console.WriteLine (proj.Name);
			Console.WriteLine ("\t\"" + proj.Name + "\"");
			foreach (var asset in proj.Assets)
			{
				Console.WriteLine ("\t" + asset.Archive.Name);
				foreach (var resource in asset.ProjectResources)
				{
					Console.WriteLine ("\t\t" + resource);
				}
			}

			proj.Export ("Content/Temp");

			Console.WriteLine ("Exported package...");
			var exportedPackage = PackageExplorer.Load ("Content/Core.bpkg");
			foreach (var asset in exportedPackage.Assets)
			{
				Console.WriteLine (asset.Root);
				foreach (var resource in asset.PackageResources)
				{
					Console.WriteLine ("\t" + resource.ToString ());
				}
			}
			
			var fireballAsset = exportedPackage.Assets["Fireball"];
			var data = fireballAsset.GetResource("Main.bhvr").LoadStream();
			
			SerializedGraph packageItem;
			
			using (var sr = new StreamReader(data))
			using (var reader = new JsonTextReader(sr))
			{
				var serializer = new JsonSerializer();
				packageItem = serializer.Deserialize<SerializedGraph>(reader);
			}

			Console.WriteLine ("Imported: " + packageItem.Name);
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