using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace RPGCore.Behaviour
{
	public class SerializedGraphInstance
	{
		public string GraphType;
		public Dictionary<LocalId, JObject> NodeInstances;

		public GraphInstance Unpack (Graph graphType)
		{
			var graph = graphType.Create (NodeInstances);

			return graph;
		}

		public string AsJson ()
		{
			var settings = new JsonSerializerSettings ();
			settings.Converters.Add (new LocalIdJsonConverter ());

			return JsonConvert.SerializeObject (this, settings);
		}
	}

	public class OutputConverter : JsonConverter
	{
		public override object ReadJson (JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}

			var type = typeof (Connection<>);

			type = type.MakeGenericType (objectType.GenericTypeArguments);

			var jobject = JObject.Load (reader);
			return jobject.ToObject (type, serializer);
		}

		public override bool CanConvert (Type objectType)
		{
			return typeof (IOutput).IsAssignableFrom (objectType)
&& !typeof (Connection).IsAssignableFrom (objectType);
		}

		public override bool CanWrite => false;

		public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotSupportedException ("JsonCreationConverter should only be used while deserializing.");
		}
	}

}
