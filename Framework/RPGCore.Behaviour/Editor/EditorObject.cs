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
		public FieldInformation Information;
		public JProperty Property;

		public EditorField(FieldInformation information, JProperty property)
		{
			Information = information;
			Property = property;
		}
	}

	public class EditorObject : IEnumerable<EditorField>
	{
		public JObject Serialized;
		public JsonObjectTypeInformation Information;

		public EditorObject(JsonObjectTypeInformation information, JObject serialized)
		{
			Information = information;
			Serialized = serialized ?? throw new ArgumentNullException();

			var fieldNames = new HashSet<string>(information.Fields.Select(f => f.Key));

			// Remove any additional fields.
			foreach (var item in serialized.Children<JProperty>().ToList())
			{
				if (!fieldNames.Contains(item.Name))
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
			foreach (var property in Serialized.Properties())
			{
				FieldInformation information = null;
				foreach (var info in Information.Fields)
				{
					if (info.Key == property.Name)
					{
						information = info.Value;
						break;
					}
				}

				yield return new EditorField(information, property);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<EditorField>)this).GetEnumerator();
		}
	}
}
