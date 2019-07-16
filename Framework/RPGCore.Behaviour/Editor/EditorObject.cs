using Newtonsoft.Json.Linq;
using RPGCore.Behaviour.Manifest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RPGCore.Behaviour.Editor
{
	public struct EditorField : IEnumerable<EditorField>
	{
		public string Name;
		public FieldInformation Info;
		public TypeInformation Type;
		public JToken Json;

		public BehaviourManifest Manifest;

		public EditorField(BehaviourManifest manifest, string name, FieldInformation info, TypeInformation type, JToken json)
			: this()
		{
			Manifest = manifest;
			Name = name;
			Info = info;
			Type = type;
			Json = json;

			if (Json.Type == JTokenType.Object
				&& Info?.Format != FieldFormat.Dictionary)
			{
				EditorObject.PopulateMissing((JObject)Json, type);
			}
		}

		public IEnumerator<EditorField> GetEnumerator()
		{
			if (Info.Format == FieldFormat.Dictionary)
			{
				foreach (var property in ((JObject)Json).Properties())
				{
					yield return new EditorField(Manifest, property.Name, Info.ValueFormat, Type, property.Value);
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

				if (field.Value.Type == "JObject")
					return new EditorField(Manifest, field.Key, field.Value, null, property);

				if (Manifest.Types.JsonTypes.TryGetValue(field.Value.Type, out var typeInformation))
				{
					return new EditorField(Manifest, field.Key, field.Value, typeInformation, property);
				}
				else if (Manifest.Types.ObjectTypes.TryGetValue(field.Value.Type, out typeInformation))
				{
					return new EditorField(Manifest, field.Key, field.Value, typeInformation, property);
				}
				else if (Manifest.Nodes.Nodes.TryGetValue(field.Value.Type, out var nodeInformation))
				{
					return new EditorField(Manifest, field.Key, field.Value, nodeInformation, property);
				}
				throw new InvalidOperationException("Could not find type " + field.Value.Type);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<EditorField>)this).GetEnumerator();
		}
	}

	public struct EditorObject : IEnumerable<EditorField>
	{
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

		public BehaviourManifest Manifest;
		public JObject Json;
		public TypeInformation Type;

		public EditorObject(BehaviourManifest manifest, TypeInformation information, JObject serialized)
		{
			Manifest = manifest;
			Type = information;
			Json = serialized ?? throw new ArgumentNullException();

			PopulateMissing(serialized, information);
		}

		public IEnumerator<EditorField> GetEnumerator()
		{
			foreach (var field in Type.Fields)
			{
				var property = Json[field.Key];

				if (Manifest.Types.JsonTypes.TryGetValue(field.Value.Type, out var typeInformation))
				{
					yield return new EditorField(Manifest, field.Key, field.Value, typeInformation, property);
				}
				else if (Manifest.Types.ObjectTypes.TryGetValue(field.Value.Type, out typeInformation))
				{
					yield return new EditorField(Manifest, field.Key, field.Value, typeInformation, property);
				}
				else if (Manifest.Nodes.Nodes.TryGetValue(field.Value.Type, out var nodeInformation))
				{
					yield return new EditorField(Manifest, field.Key, field.Value, nodeInformation, property);
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

				if (Manifest.Types.JsonTypes.TryGetValue(field.Value.Type, out var typeInformation))
				{
					return new EditorField(Manifest, field.Key, field.Value, typeInformation, property);
				}
				else if (Manifest.Types.ObjectTypes.TryGetValue(field.Value.Type, out typeInformation))
				{
					return new EditorField(Manifest, field.Key, field.Value, typeInformation, property);
				}
				else if (Manifest.Nodes.Nodes.TryGetValue(field.Value.Type, out var nodeInformation))
				{
					return new EditorField(Manifest, field.Key, field.Value, nodeInformation, property);
				}
				throw new InvalidOperationException();
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<EditorField>)this).GetEnumerator();
		}
	}
}
