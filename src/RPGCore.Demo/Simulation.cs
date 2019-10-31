using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGCore.Behaviour;
using RPGCore.Behaviour.Editor;
using RPGCore.Behaviour.Manifest;
using RPGCore.Demo.Nodes;
using RPGCore.Packages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace RPGCore.Demo
{
	public sealed class Simulation
	{
		public void Start ()
		{
			var nodes = NodeManifest.Construct (new Type[] { typeof (AddNode), typeof (RollNode), typeof (OutputValueNode) });
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
			foreach (var resource in ((IPackageExplorer)proj).Resources)
			{
				Console.WriteLine ("\t" + resource.FullName);
			}

			var editorTargetResource = proj.Resources["Tutorial Gamerules/Main.bhvr"];
			var editorTargetData = editorTargetResource.LoadStream ();

			JObject editorTarget;

			var serializer = new JsonSerializer ();

			using (var sr = new StreamReader (editorTargetData))
			using (var reader = new JsonTextReader (sr))
			{
				editorTarget = JObject.Load (reader);
			}

			var editor = new EditorSession (manifest, editorTarget, "SerializedGraph", serializer);

			foreach (var node in editor.Root["Nodes"])
			{
				var nodeData = node["Data"];

				foreach (var field in nodeData)
				{
					Console.WriteLine ($"{field}");
					if (field.Name == "MaxValue")
					{
						field.SetValue (field.GetValue<int> () + 10);
						field.ApplyModifiedProperties ();

						field.SetValue (field.GetValue<int> ());
						field.ApplyModifiedProperties ();
					}
					else if (field.Name == "ValueB")
					{
						Console.WriteLine (field.GetValue<LocalPropertyId> ());
					}
				}
			}

			using (var file = editorTargetResource.WriteStream ())
			using (var jsonWriter = new JsonTextWriter (file)
			{
				Formatting = Formatting.Indented
			})
			{
				serializer.Serialize (jsonWriter, editorTarget);
			}

			Console.WriteLine (new DirectoryInfo ("Content/Temp").FullName);

			var consoleRenderer = new BuildConsoleRenderer ();

			var buildPipeline = new BuildPipeline ()
			{
				Exporters = new List<ResourceExporter> ()
				{
					new BhvrExporter()
				},
				BuildActions = new List<IBuildAction> ()
				{
					consoleRenderer
				}
			};

			consoleRenderer.DrawProgressBar (32);
			proj.Export (buildPipeline, "Content/Temp");

			Console.WriteLine ("Exported package...");
			var exportedPackage = PackageExplorer.Load ("Content/Temp/Core.bpkg");

			var fireballAsset = exportedPackage.Resources["Fireball/Main.bhvr"];
			var data = fireballAsset.LoadStream ();

			SerializedGraph packageItem;

			using (var sr = new StreamReader (data))
			using (var reader = new JsonTextReader (sr))
			{
				packageItem = serializer.Deserialize<SerializedGraph> (reader);
			}

			Console.WriteLine ("Imported: " + packageItem.Name);
			var unpackedGraph = packageItem.Unpack ();

			Console.WriteLine ("Running Simulation...");

			var player = new DemoPlayer ();

			IGraphInstance instancedItem = unpackedGraph.Create ();
			instancedItem.Setup ();
			instancedItem.SetInput (player);
			for (int i = 0; i < 5; i++)
			{
				Thread.Sleep (100);
				player.Health.Value -= 10;
			}
			instancedItem.Remove ();

			var packedInstance = ((GraphInstance)instancedItem).Pack ();
			string serializedGraph = packedInstance.AsJson ();
			Console.WriteLine (serializedGraph);

			var deserialized = JsonConvert.DeserializeObject<SerializedGraphInstance> (serializedGraph);
			var unpackedInstance = deserialized.Unpack (unpackedGraph);

			unpackedInstance.SetInput (player);
			unpackedInstance.Setup ();
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
