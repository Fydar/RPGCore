using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace RPGCore.Behaviour
{
	public sealed class SerializedGraphInstance
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

	public sealed class OutputConverter : JsonConverter
	{
		public override object ReadJson (JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}

			var connectionType = typeof (Connection<>).MakeGenericType (objectType.GenericTypeArguments);
			var outputType = typeof (Output<>).MakeGenericType (objectType.GenericTypeArguments);

			object connectionObject = JObject.Load (reader).ToObject (connectionType, serializer);
			object outputObject = Activator.CreateInstance (outputType, connectionObject);

			return outputObject;
		}

		public override bool CanConvert (Type objectType)
		{
			return IsSubclassOfRawGeneric (typeof (Output<>), objectType)
				&& !typeof (Connection).IsAssignableFrom (objectType);
		}

		public override bool CanWrite => false;

		public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotSupportedException ("JsonCreationConverter should only be used while deserializing.");
		}

		private static bool IsSubclassOfRawGeneric (Type generic, Type toCheck)
		{
			while (toCheck != null && toCheck != typeof (object))
			{
				var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition () : toCheck;
				if (generic == cur)
				{
					return true;
				}
				toCheck = toCheck.BaseType;
			}
			return false;
		}
	}

}
