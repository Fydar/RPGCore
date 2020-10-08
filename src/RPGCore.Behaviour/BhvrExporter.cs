using Newtonsoft.Json;
using RPGCore.Packages;
using RPGCore.Packages.Archives;
using System.IO;

namespace RPGCore.Behaviour
{
	public class BhvrExporter : ResourceExporter
	{
		public override bool CanExport(IResource resource)
		{
			return resource.Extension == ".bhvr";
		}

		public override void BuildResource(IResource resource, IArchiveDirectory destination)
		{
			var serializer = new JsonSerializer();
			SerializedGraph serializedGraph;

			using (var sr = new StreamReader(resource.Content.LoadStream()))
			using (var reader = new JsonTextReader(sr))
			{
				serializedGraph = serializer.Deserialize<SerializedGraph>(reader);
			}

			foreach (var node in serializedGraph.Nodes)
			{
				node.Value.Editor = default;
			}

			var entry = destination.Files.GetFile(resource.Name);
			using var zipStream = entry.OpenWrite();
			using var streamWriter = new StreamWriter(zipStream);
			serializer.Serialize(streamWriter, serializedGraph);
		}
	}
}
