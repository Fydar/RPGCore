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
		public TypeInformation Type;
		public JToken Json;
		
		private BehaviourManifest Manifest;

		public EditorField(BehaviourManifest manifest, string name, FieldInformation info, TypeInformation type, JToken json)
			: this()
		{
			Manifest = manifest;
			Name = name;
			Info = info;
			Type = type;
			Json = json;

			if (Json.Type == JTokenType.Object)
			{
				EditorObject.PopulateMissing((JObject)Json, type);
			}
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
		public JObject Serialized;
		public TypeInformation Information;

		public EditorObject(BehaviourManifest manifest, TypeInformation information, JObject serialized)
		{
			Manifest = manifest;
			Information = information;
			Serialized = serialized ?? throw new ArgumentNullException();

			PopulateMissing(serialized, information);
		}

		public IEnumerator<EditorField> GetEnumerator()
		{
			foreach (var field in Information.Fields)
			{
				var property = Serialized[field.Key];

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

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<EditorField>)this).GetEnumerator();
		}
	}
}
