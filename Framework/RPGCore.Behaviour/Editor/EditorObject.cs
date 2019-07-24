using Newtonsoft.Json.Linq;
using RPGCore.Behaviour.Manifest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RPGCore.Behaviour.Editor
{
	public class EditorField : IEnumerable<EditorField>
	{
		public string Name;
		public FieldInformation Field;
		public TypeInformation Type;
		public JToken Json;

		public EditorSession Session;

		public EditorField(EditorSession session, JToken json, string name, FieldInformation info)
		{
			Session = session;
			Name = name;
			Field = info;
			Json = json;

			Type = session.GetTypeInformation(info.Type);

			if (Json.Type == JTokenType.Object
				&& Field?.Format != FieldFormat.Dictionary)
			{
				PopulateMissing((JObject)Json, Type);
			}
		}

		public IEnumerator<EditorField> GetEnumerator()
		{
			if (Field.Format == FieldFormat.Dictionary)
			{
				foreach (var property in ((JObject)Json).Properties())
				{
					yield return new EditorField(Session, property.Value, property.Name, Field.ValueFormat);
				}
			}
			else
			{
				foreach (var field in Type.Fields)
				{
					yield return this[field.Key];
				}
			}
		}

		public EditorField this[string key]
		{
			get
			{
				KeyValuePair<string, FieldInformation> field = new KeyValuePair<string, FieldInformation>();
				foreach (var potentialField in Type.Fields)
				{
					if (potentialField.Key == key)
					{
						field = potentialField;
						break;
					}
				}

				var property = Json[key];

				return new EditorField(Session, property, field.Key, field.Value);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<EditorField>)this).GetEnumerator();
		}
		
		public static void PopulateMissing(JObject serialized, TypeInformation information)
		{
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
	}

	public class EditorSession
	{
		public BehaviourManifest Manifest;
		public JObject Json;
		public TypeInformation Type;

		public EditorField Root;

		public EditorSession(BehaviourManifest manifest, object instance)
		{
			Manifest = manifest;
			Root = new EditorField(this, JObject.FromObject(instance), "root", new FieldInformation()
			{
				Type = instance.GetType().FullName
			});
		}

		public EditorSession(BehaviourManifest manifest, JObject instance, string type)
		{
			Manifest = manifest;
			Root = new EditorField(this, instance, "root", new FieldInformation()
			{
				Type = type
			});
		}

		public static TypeInformation GetTypeInformation(BehaviourManifest manifest, string type)
		{
			if (manifest.Types.JsonTypes.TryGetValue(type, out var jsonType))
			{
				return jsonType;
			}
			if (manifest.Types.ObjectTypes.TryGetValue(type, out var objectType))
			{
				return objectType;
			}
			if (manifest.Nodes.Nodes.TryGetValue(type, out var nodeType))
			{
				return nodeType;
			}
			return null;
		}

		public TypeInformation GetTypeInformation(string type)
		{
			return GetTypeInformation(Manifest, type);
		}
	}
}
