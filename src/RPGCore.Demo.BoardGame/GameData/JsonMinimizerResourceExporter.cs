using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGCore.Packages;
using RPGCore.Packages.Archives;
using System.IO;

namespace RPGCore.Demo.BoardGame
{
	public class JsonMinimizerResourceExporter : ResourceExporter
	{
		public override bool CanExport(IResource resource)
		{
			return resource.Extension == ".json";
		}

		public override void BuildResource(IResource resource, IArchiveEntry contentEntry)
		{
			var serializer = new JsonSerializer()
			{
				Formatting = Formatting.None
			};

			JObject document;
			using (var sr = new StreamReader(resource.Content.LoadStream()))
			using (var reader = new JsonTextReader(sr))
			{
				document = serializer.Deserialize<JObject>(reader);
			}

			using var zipStream = contentEntry.OpenWrite();
			using var streamWriter = new StreamWriter(zipStream);
			serializer.Serialize(streamWriter, document);
		}
	}
}
