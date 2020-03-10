using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGCore.Packages;
using System.IO;

namespace RPGCore.Demo.BoardGame
{
	public class JsonMinimizerResourceExporter : ResourceExporter
	{
		public override string ExportExtensions => "json";

		public override void BuildResource(IResource resource, Stream writer)
		{
			var serializer = new JsonSerializer()
			{
				Formatting = Formatting.None
			};

			JObject document;
			using (var sr = new StreamReader(resource.LoadStream()))
			using (var reader = new JsonTextReader(sr))
			{
				document = serializer.Deserialize<JObject>(reader);
			}

			using var streamWriter = new StreamWriter(writer);
			serializer.Serialize(streamWriter, document);
		}
	}
}
