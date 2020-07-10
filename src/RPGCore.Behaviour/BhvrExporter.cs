using Newtonsoft.Json;
using RPGCore.Packages;
using System.IO;
using System.IO.Compression;

namespace RPGCore.Behaviour
{
	public class BhvrExporter : ResourceExporter
	{
		public override bool CanExport(IResource resource)
		{
			return resource.Extension == ".bhvr";
		}

		public override void BuildResource(IResource resource, ZipArchiveEntry contentEntry)
		{
			var serializer = new JsonSerializer();
			SerializedGraph serializedGraph;

			using (var sr = new StreamReader(resource.LoadStream()))
			using (var reader = new JsonTextReader(sr))
			{
				serializedGraph = serializer.Deserialize<SerializedGraph>(reader);
			}

			foreach (var node in serializedGraph.Nodes)
			{
				node.Value.Editor = default;
			}

			using var zipStream = contentEntry.Open();
			using var streamWriter = new StreamWriter(zipStream);
			serializer.Serialize(streamWriter, serializedGraph);
		}
	}
}
