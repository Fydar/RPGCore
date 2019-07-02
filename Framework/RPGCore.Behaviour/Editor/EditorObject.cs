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
		public FieldInformation Information;
		public JValue Property;

		public EditorField(string name, FieldInformation information, JValue property)
		{
			Name = name;
			Information = information;
			Property = property;
		}
	}

	public class EditorObject : IEnumerable<EditorField>
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
					yield return new EditorField(field.Key, field.Value, (JValue)property);
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<EditorField>)this).GetEnumerator();
		}
	}
}
