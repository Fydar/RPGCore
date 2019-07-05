using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGCore.Behaviour.Manifest;
using RPGCore.Packages;

namespace RPGCore.Behaviour.Editor
{
	public struct EditorField
	{
		public string Name;
		public FieldInformation Info;

		public JsonValueTypeInformation ValueTypeInfo;
		public JValue JsonValue;

		public JsonObjectTypeInformation ObjectTypeInfo;
		public JObject JsonObject;

		public EditorField(string name, FieldInformation info, JsonValueTypeInformation typeInfo, JValue jsonValue)
			: this()
		{
			Name = name;
			Info = info;
			ValueTypeInfo = typeInfo;
			JsonValue = jsonValue;
		}

		public EditorField(string name, FieldInformation info, JsonObjectTypeInformation typeInfo, JObject jsonObject)
			: this()
		{
			Name = name;
			Info = info;
			ObjectTypeInfo = typeInfo;
			JsonObject = jsonObject;
		}
	}

	public struct EditorObject : IEnumerable<EditorField>
	{
		public BehaviourManifest Manifest;
		public JObject Serialized;
		public JsonObjectTypeInformation Information;

		public EditorObject(BehaviourManifest manifest, JsonObjectTypeInformation information, JObject serialized)
		{
			Manifest = manifest;
			Information = information;
			Serialized = serialized ?? throw new ArgumentNullException();

			// Remove any additional fields.
			foreach (var item in serialized.Children<JProperty>().ToList())
			{
				if (!information.Fields.Keys.Contains(item.Name))
				{
					item.Remove();
				}
			}

			// Populate missing fields with default values.
			foreach (var field in information.Fields)
			{
				if (!serialized.ContainsKey(field.Key))
				{
					serialized.Add(field.Key, field.Value.DefaultValue);
				}
			}
		}

		public IEnumerator<EditorField> GetEnumerator()
		{
			foreach (var field in Information.Fields)
			{
				var property = Serialized[field.Key];

				if (Manifest.Types.JsonTypes.TryGetValue(field.Value.Type, out var typeInformation))
				{
					yield return new EditorField(field.Key, field.Value, typeInformation, (JValue)property);
				}
				else if (Manifest.Types.ObjectTypes.TryGetValue(field.Value.Type, out var objectTypeInfo))
				{
					yield return new EditorField(field.Key, field.Value, objectTypeInfo, (JObject)property);
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<EditorField>)this).GetEnumerator();
		}
	}
}
