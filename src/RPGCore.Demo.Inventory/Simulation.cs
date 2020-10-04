using Newtonsoft.Json;
using RPGCore.Behaviour;
using RPGCore.Packages;
using RPGCore.Packages.Archives;
using System;
using System.IO;
using System.Threading;

namespace RPGCore.Demo.Inventory
{
	public sealed class Simulation
	{
		void RenderDirectory(IReadOnlyArchiveDirectory archiveDirectory, int indent = 0)
		{
			string indentString = new string(' ', indent * 2);

			foreach (var subdirectory in archiveDirectory.Directories)
			{
				Console.WriteLine($"{indentString}> {subdirectory.Name}");
				RenderDirectory(subdirectory, indent + 1);
			}
			foreach (var file in archiveDirectory.Files)
			{
				Console.WriteLine($"{indentString}- {file.Name}");
			}
		}

		public void Start()
		{
			var serializer = new JsonSerializer();

			Console.WriteLine("Importing Graph...");

			var importPipeline = ImportPipeline.Create()
				.UseProcessor(new TagAllProjectResourceImporter())
				.Build();

			var projectExplorer = ProjectExplorer.Load("Content/Core", importPipeline);

			Console.WriteLine("Building project files...");
			RenderDirectory(projectExplorer.Archive.RootDirectory);

			var consoleRenderer = new BuildConsoleRenderer();

			var buildPipeline = new BuildPipeline();
			buildPipeline.Exporters.Add(new BhvrExporter());
			buildPipeline.BuildActions.Add(consoleRenderer);

			consoleRenderer.DrawProgressBar(32);
			projectExplorer.ExportZippedToDirectory(buildPipeline, "Content/Temp");

			Console.WriteLine("Exported package...");
			var exportedPackage = PackageExplorer.LoadFromFileAsync("Content/Temp/Core.bpkg").Result;
			RenderDirectory(exportedPackage.Source);

			var fireballAsset = exportedPackage.Resources["Fireball/Main.json"];
			var data = fireballAsset.Content.LoadStream();

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
