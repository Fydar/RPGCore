using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace RPGCore.Behaviour
{
	public sealed class SerializedNode
	{
		public string Type;
		public JObject Data;
		public PackageNodeEditor Editor;

		public Node UnpackInputs (LocalId id, List<string> connectionIds, ref int outputCounter)
		{
			var nodeType = System.Type.GetType (Type);

			var jsonSerializer = new JsonSerializer ();

			jsonSerializer.Converters.Add (new InputSocketConverter (connectionIds));
			jsonSerializer.Converters.Add (new LocalIdJsonConverter ());
			object nodeObject = Data.ToObject (nodeType, jsonSerializer);

			var node = (Node)nodeObject;
			node.Id = id;

			return node;
		}

		public static void UnpackOutputs (List<string> connectionIds, Node node)
		{
			var nodeType = node.GetType ();

			foreach (var field in nodeType.GetFields (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
			{
				if (field.FieldType == typeof (OutputSocket))
				{
					int connectionId = connectionIds.IndexOf (node.Id + "." + field.Name);
					field.SetValue (node, new OutputSocket (connectionId));
				}
			}
		}
	}

	internal sealed class InputSocketConverter : JsonConverter
	{
		private readonly List<string> MappedInputs;

		public override bool CanWrite => false;

		public override bool CanConvert (Type objectType)
		{
			return (objectType == typeof (InputSocket));
		}

		public InputSocketConverter (List<string> mappedInputs)
		{
			MappedInputs = mappedInputs;
		}

		public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new InvalidOperationException ();
		}

		public override object ReadJson (JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.Value == null)
			{
				return new InputSocket ();
			}

			string inputSource = reader.Value.ToString ();

			int connectionId = MappedInputs.IndexOf (inputSource);
			if (connectionId == -1)
			{
				connectionId = MappedInputs.Count;
				MappedInputs.Add (inputSource);
			}

			return new InputSocket (connectionId);
		}
	}
}
