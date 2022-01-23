using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RPGCore.Behaviour;

public sealed class SerializedGraphInstance
{
	public Dictionary<LocalId, JObject> NodeInstances;

	public GraphInstance Unpack(Graph graph)
	{
		var graphInstance = graph.Create(NodeInstances);

		return graphInstance;
	}

	public string AsJson()
	{
		var settings = new JsonSerializerSettings();
		settings.Converters.Add(new LocalIdJsonConverter());
		settings.Converters.Add(new SerializedGraphInstanceProxyConverter(null));

		return JsonConvert.SerializeObject(this, settings);
	}
}

public sealed class SerializedGraphInstanceProxyConverter : JsonConverter
{
	private readonly Graph graph;

	public override bool CanRead => true;
	public override bool CanWrite => true;

	public SerializedGraphInstanceProxyConverter(Graph graph)
	{
		this.graph = graph;
	}

	public override bool CanConvert(Type objectType)
	{
		return typeof(IGraphInstance).IsAssignableFrom(objectType);
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		var jObject = JObject.Load(reader);
		var serializedGraphInstance = jObject.ToObject<SerializedGraphInstance>(serializer);

		var result = serializedGraphInstance.Unpack(graph.SubGraphs.Values.First());

		return result;
	}

	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		var graphInstance = (GraphInstance)value;
		var serializedGraphInstance = graphInstance.Pack();

		var token = JToken.FromObject(serializedGraphInstance, serializer);
		token.WriteTo(writer);
	}
}

public sealed class OutputConverter : JsonConverter
{
	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		if (reader.TokenType == JsonToken.Null)
		{
			return null;
		}

		var connectionType = typeof(OutputConnection<>).MakeGenericType(objectType.GenericTypeArguments);
		var outputType = typeof(Output<>).MakeGenericType(objectType.GenericTypeArguments);

		object connectionObject = JObject.Load(reader).ToObject(connectionType, serializer);
		object outputObject = Activator.CreateInstance(outputType, connectionObject);

		return outputObject;
	}

	public override bool CanConvert(Type objectType)
	{
		return IsSubclassOfRawGeneric(typeof(Output<>), objectType)
			&& !typeof(IConnection).IsAssignableFrom(objectType);
	}

	public override bool CanWrite => false;

	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		throw new NotSupportedException("JsonCreationConverter should only be used while deserializing.");
	}

	private static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
	{
		while (toCheck != null && toCheck != typeof(object))
		{
			var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
			if (generic == cur)
			{
				return true;
			}
			toCheck = toCheck.BaseType;
		}
		return false;
	}
}
