using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace RPGCore.Behaviour
{
	public class SerializedNode
	{
		public string Type;
		public JObject Data;
		public PackageNodeEditor _Editor;

		public Node Unpack (List<string> outputIds, ref int outputCounter)
		{
			var nodeType = System.Type.GetType (Type);

			var jsonSerializer = new JsonSerializer ();
			jsonSerializer.Converters.Add (new InputSocketConverter (outputIds));
			object nodeObject = Data.ToObject(nodeType, jsonSerializer);

			foreach (var field in nodeType.GetFields (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
			{
				if (field.FieldType == typeof (OutputSocket))
				{
					field.SetValue (nodeObject, new OutputSocket (++outputCounter));
				}
			}

			return (Node)nodeObject;
		}
	}

	class InputSocketConverter : JsonConverter
	{
        private readonly List<string> Ids;

        public override bool CanWrite
		{
			get { return false; }
		}

		public override bool CanConvert(Type objectType)
		{
			return (objectType == typeof(InputSocket));
		}

		public InputSocketConverter (List<string> ids)
		{
            Ids = ids;
        }

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new InvalidOperationException();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			return new InputSocket(Ids.IndexOf(reader.Value.ToString()));
		}
	}
}
