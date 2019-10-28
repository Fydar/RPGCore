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

		public Node UnpackInputs (Type nodeType, LocalId id, HashSet<LocalPropertyId> validOutputs, List<LocalPropertyId> connectionIds)
		{
			var jsonSerializer = new JsonSerializer ();

			jsonSerializer.Converters.Add (new InputSocketConverter (validOutputs, connectionIds));
			jsonSerializer.Converters.Add (new LocalIdJsonConverter ());
			object nodeObject = Data.ToObject (nodeType, jsonSerializer);

			var node = (Node)nodeObject;
			node.Id = id;

			return node;
		}

		public static void UnpackOutputs (List<LocalPropertyId> connectionIds, Node node)
		{
			var nodeType = node.GetType ();

			foreach (var field in nodeType.GetFields (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
			{
				if (field.FieldType == typeof (OutputSocket))
				{
					int connectionId = connectionIds.IndexOf (new LocalPropertyId (node.Id, field.Name));
					field.SetValue (node, new OutputSocket (connectionId));
				}
			}
		}
	}

	internal sealed class InputSocketConverter : JsonConverter
	{
		private readonly HashSet<LocalPropertyId> ValidOutputs;
		private readonly List<LocalPropertyId> MappedInputs;

		public override bool CanWrite => false;

		public override bool CanConvert (Type objectType)
		{
			return (objectType == typeof (InputSocket));
		}

		public InputSocketConverter (HashSet<LocalPropertyId> validOutputs, List<LocalPropertyId> mappedInputs)
		{
			ValidOutputs = validOutputs;
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

			var inputSource = new LocalPropertyId (reader.Value.ToString ());

			if (!ValidOutputs.Contains(inputSource))
			{
				Console.WriteLine ($"Ignoring desired input of \"{inputSource}\" as it is not valid.");
				return new InputSocket ();
			}

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
