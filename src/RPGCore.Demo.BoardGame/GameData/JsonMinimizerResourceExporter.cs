using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGCore.FileTree;
using RPGCore.Packages;
using RPGCore.Projects;
using System.IO;

namespace RPGCore.Demo.BoardGame
{
	public class JsonMinimizerResourceExporter : ResourceExporter
	{
		public override bool CanExport(IResource resource)
		{
			return resource.Extension == ".json";
		}

		public override void BuildResource(IResource resource, IArchiveDirectory destination)
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


			var entry = destination.Files.GetOrCreateFile($"data/{resource.FullName}");
			using var zipStream = entry.OpenWrite();
			using var streamWriter = new StreamWriter(zipStream);
			serializer.Serialize(streamWriter, document);
		}
	}
}
