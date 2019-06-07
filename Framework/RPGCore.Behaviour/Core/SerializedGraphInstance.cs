using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RPGCore.Behaviour
{
	public class SerializedGraphInstance
	{
		public string GraphType;
		public Dictionary<LocalId, JObject> NodeInstances;

		public GraphInstance Unpack(Graph graphType)
		{
			var graph = graphType.Create(NodeInstances);

			return graph;
		}

		public string AsJson()
		{
			var settings = new JsonSerializerSettings();
			settings.Converters.Add (new LocalIdJsonConverter());

			return JsonConvert.SerializeObject(this, settings);
		}
	}
}
