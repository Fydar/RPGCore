using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RPGCore.Packages;

namespace RPGCore.Behaviour
{
	public class BhvrImporter : ResourceImporter
	{
		public override string ImportExtensions => "bhvr";

		public override void BuildResource(IResource resource, Stream writer)
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
				node.Value.Editor = default(PackageNodeEditor);
			}

			using (var streamWriter = new StreamWriter(writer))
			{
				serializer.Serialize(streamWriter, serializedGraph);
			}
		}
	}
}
