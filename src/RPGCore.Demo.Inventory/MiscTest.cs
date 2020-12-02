using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGCore.Behaviour;
using RPGCore.DataEditor;
using RPGCore.DataEditor.Manifest;
using RPGCore.Packages;
using RPGCore.Projects;
using System;
using System.IO;
using System.Threading;

namespace RPGCore.Demo.Inventory
{
	public static class MiscTest
	{
		public static void Run()
		{
			var manifest = BehaviourManifest.CreateFromAppDomain(AppDomain.CurrentDomain);

			File.WriteAllText("Content/RPGCoreMath.bmfst", manifest.ToString());

			Console.WriteLine("Importing Graph...");

			var importPipeline = ImportPipeline.Create().Build();

			var proj = ProjectExplorer.Load("Content/Core", importPipeline);
			Console.WriteLine(proj.Definition.Properties.Name);
			Console.WriteLine($"\t\"{proj.Definition.Properties.Name}\"");
			foreach (var resource in ((IExplorer)proj).Resources)
			{
				Console.WriteLine($"\t{resource.FullName}");
			}

			var editorTargetResource = proj.Resources["Fireball/Main.json"];
			var editorTargetData = editorTargetResource.Content.LoadStream();

			JObject editorTarget;

			var serializer = new JsonSerializer();

			using (var sr = new StreamReader(editorTargetData))
			using (var reader = new JsonTextReader(sr))
			{
				editorTarget = JObject.Load(reader);
			}

			var editor = new EditorSession(manifest, editorTarget, "SerializedGraph", serializer);

			foreach (var node in (editor.Root.Fields["Nodes"].Value as EditorObject).Fields.Values)
			{
				var nodeData = (node.Value as EditorObject).Fields["Data"];

				foreach (var field in (nodeData.Value as EditorObject).Fields.Values)
				{
					var editableValue = field.Value as EditorValue;

					Console.WriteLine($"{field}");
					if (field.Field.Name == "MaxValue")
					{

						editableValue.SetValue(editableValue.GetValue<int>() + 10);
						editableValue.ApplyModifiedProperties();

						editableValue.SetValue(editableValue.GetValue<int>());
						editableValue.ApplyModifiedProperties();
					}
					else if (field.Field.Name == "ValueB")
					{
						Console.WriteLine(editableValue.GetValue<LocalPropertyId>());
					}
				}
			}

			using (var file = editorTargetResource.Content.OpenWrite())
			using (var sr = new StreamWriter(file))
			using (var jsonWriter = new JsonTextWriter(sr)
			{
				Formatting = Formatting.Indented
			})
			{
				serializer.Serialize(jsonWriter, editorTarget);
			}

			Console.WriteLine(new DirectoryInfo("Content/Temp").FullName);

			var consoleRenderer = new BuildConsoleRenderer();

			var buildPipeline = new BuildPipeline();
			buildPipeline.Exporters.Add(new BhvrExporter());
			buildPipeline.BuildActions.Add(consoleRenderer);

			consoleRenderer.DrawProgressBar(32);
			proj.ExportZippedToDirectory(buildPipeline, "Content/Temp");

			Console.WriteLine("Exported package...");
			var exportedPackage = PackageExplorer.LoadFromFileAsync("Content/Temp/Core.bpkg").Result;

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

			// var packedInstance = ((GraphInstance)instancedItem).Pack ();
			// string serializedGraph = packedInstance.AsJson ();
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
