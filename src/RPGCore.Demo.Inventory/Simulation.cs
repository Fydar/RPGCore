using Newtonsoft.Json;
using RPGCore.Behaviour;
using RPGCore.Behaviour.Manifest;
using RPGCore.Demo.Inventory.Nodes;
using RPGCore.Packages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace RPGCore.Demo.Inventory
{
	public sealed class Simulation
	{
		public void Start()
		{
			var manifest = BehaviourManifest.CreateFromAppDomain(AppDomain.CurrentDomain);

			var serializer = new JsonSerializer();

			Console.WriteLine("Importing Graph...");

			var importPipeline = new ImportPipeline();
			importPipeline.ImportProcessors.Add(
				new TagAllProjectResourceImporter());

			var projectExplorer = ProjectExplorer.Load("Content/Core", importPipeline);

			var consoleRenderer = new BuildConsoleRenderer();

			var buildPipeline = new BuildPipeline()
			{
				ImportPipeline = importPipeline,
				Exporters = new List<ResourceExporter>()
				{
					new BhvrExporter()
				},
				BuildActions = new List<IBuildAction>()
				{
					consoleRenderer
				}
			};
			consoleRenderer.DrawProgressBar(32);
			projectExplorer.Export(buildPipeline, "Content/Temp");

			Console.WriteLine("Exported package...");
			var exportedPackage = PackageExplorer.Load("Content/Temp/Core.bpkg");

			var fireballAsset = exportedPackage.Resources["Fireball/Main.bhvr"];
			var data = fireballAsset.LoadStream();

			SerializedGraph packageItem;
			using (var sr = new StreamReader(data))
			using (var reader = new JsonTextReader(sr))
			{
				packageItem = serializer.Deserialize<SerializedGraph>(reader);
			}

			Console.WriteLine("Imported: " + fireballAsset.Name);
			var unpackedGraph = packageItem.Unpack();

			Console.WriteLine("Running Simulation...");

			var player = new DemoPlayer();

			IGraphInstance instancedItem = unpackedGraph.Create();
			instancedItem.Setup();
			instancedItem.SetInput(player);
			for (int i = 0; i < 5; i++)
			{
				Thread.Sleep(100);
				player.Health.Value -= 10;
			}
			instancedItem.Remove();

			var settings = new JsonSerializerSettings();
			settings.Converters.Add(new LocalIdJsonConverter());
			settings.Converters.Add(new SerializedGraphInstanceProxyConverter(null));

			string serializedGraph = JsonConvert.SerializeObject(instancedItem, settings);
			Console.WriteLine(serializedGraph);

			var deserialized = JsonConvert.DeserializeObject<SerializedGraphInstance>(serializedGraph);
			var unpackedInstance = deserialized.Unpack(unpackedGraph);

			unpackedInstance.SetInput(player);
			unpackedInstance.Setup();
			for (int i = 0; i < 5; i++)
			{
				Thread.Sleep(100);
				player.Health.Value -= 20;
			}
			unpackedInstance.Remove();

			for (int i = 0; i < 5; i++)
			{
				Thread.Sleep(100);
				player.Health.Value -= 20;
			}
		}
	}
}
