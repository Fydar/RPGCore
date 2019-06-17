using Newtonsoft.Json;
using RPGCore.Behaviour.Manifest;
using RPGCore.Packages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace RPGCore.Behaviour
{	
	public class Simulation
	{
		public void Start ()
		{
			var nodes = NodeManifest.Construct (new Type[] { typeof (AddNode), typeof (RollNode) });
			var types = TypeManifest.ConstructBaseTypes ();

			var manifest = new BehaviourManifest ()
			{
				Nodes = nodes,
				Types = types
			};

			File.WriteAllText ("Content/RPGCoreMath.bmfst", manifest.ToString ());

			Console.WriteLine ("Importing Graph...");

			var proj = ProjectExplorer.Load ("Content/Tutorial", new List<ResourceImporter>()
			{
				new BhvrImporter()
			});
			Console.WriteLine (proj.Name);
			Console.WriteLine ("\t\"" + proj.Name + "\"");
			foreach (var resource in proj.Resources)
			{
				Console.WriteLine ("\t" + resource.FullName);
			}

			Console.WriteLine(new DirectoryInfo("Content/Temp").FullName);
			proj.Export ("Content/Temp");

			Console.WriteLine ("Exported package...");
			var exportedPackage = PackageExplorer.Load ("Content/Temp/Core.bpkg");
			
			var fireballAsset = exportedPackage.Resources["Fireball/Main.bhvr"];
			var data = fireballAsset.LoadStream();
			
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

			var packedInstance = ((GraphInstance)instancedItem).Pack();
			string serializedGraph = packedInstance.AsJson();
			Console.WriteLine(serializedGraph);
			
			var deserialized = JsonConvert.DeserializeObject<SerializedGraphInstance>(serializedGraph);
			var unpackedInstance = deserialized.Unpack(unpackedGraph);

			unpackedInstance.Setup(player);
			for (int i = 0; i < 5; i++)
			{
				Thread.Sleep (100);
				player.Health.Value -= 20;
			}
			unpackedInstance.Remove ();
			
			for (int i = 0; i < 5; i++)
			{
				Thread.Sleep (100);
				player.Health.Value -= 20;
			}
		}
	}
}