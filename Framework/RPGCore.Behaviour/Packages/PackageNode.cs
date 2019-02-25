using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace RPGCore.Behaviour.Packages
{
	public class PackageNode
	{
		public string Type;
		public object Data;
		public PackageNodeEditor _Editor;

		public Node Unpack (List<string> outputIds, ref int outputCounter)
		{
			var nodeType = System.Type.GetType (Type);

			var jsonSerializer = new JsonSerializer ();
			jsonSerializer.Converters.Add (new InputSocketConverter (outputIds));
			object nodeObject = jsonSerializer.Deserialize (new System.IO.StringReader (JsonConvert.SerializeObject (Data)), nodeType);

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

	public class InputSocketConverter : JsonCreationConverter<InputSocket>
	{
		private List<string> NodeIds = new List<string> ();

		public InputSocketConverter (List<string> nodeIds)
		{
			NodeIds = nodeIds;
		}

		protected override InputSocket Create (Type objectType, string jObject)
		{
			int nodeId = NodeIds.IndexOf (jObject);

			return new InputSocket (nodeId);
		}
	}

	public abstract class JsonCreationConverter<T> : JsonConverter
	{
		protected abstract T Create (Type objectType, string jObject);

		public override bool CanConvert (Type objectType)
		{
			return typeof (T).IsAssignableFrom (objectType);
		}

		public override bool CanWrite
		{
			get { return false; }
		}

		public override object ReadJson (JsonReader reader,
										Type objectType,
										object existingValue,
										JsonSerializer serializer)
		{
			// Load JObject from stream
			string jObject = JToken.Load (reader).ToString ();

			// Create target object based on JObject
			T target = Create (objectType, jObject);

			// Populate the object properties
			// serializer.Populate(jObject.CreateReader(), target);

			return target;
		}

		public override void WriteJson (JsonWriter writer, object target, JsonSerializer serializer)
		{

		}
	}
}
