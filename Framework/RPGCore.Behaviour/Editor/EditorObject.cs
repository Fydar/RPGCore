using Newtonsoft.Json.Linq;
using RPGCore.Behaviour.Manifest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RPGCore.Behaviour.Editor
{
	public struct EditorField
	{
		public FieldInformation Information;
		public JProperty Property;

		public EditorField (FieldInformation information, JProperty property)
		{
			Information = information;
			Property = property;
		}
	}

	public class EditorObject : IEnumerable<EditorField>
	{
		public JObject Serialized;
		public EditableTargetInformation Information;

		public EditorObject (EditableTargetInformation information, JObject serialized)
		{
			Information = information;
			Serialized = serialized ?? throw new ArgumentNullException ();

			var fieldNames = new HashSet<string> (information.Fields.Select (f => f.Name));

			// Remove any additional fields.
			foreach (var item in serialized.Children<JProperty> ().ToList ())
			{
				if (!fieldNames.Contains (item.Name))
				{
					item.Remove ();
				}
			}

			// Populate missing fields with default values.
			foreach (var field in information.Fields)
			{
				if (!serialized.ContainsKey (field.Name))
				{
					serialized.Add (field.Name, field.DefaultValue);
				}
			}
		}

		public IEnumerator<EditorField> GetEnumerator ()
		{
			foreach (var property in Serialized.Properties ())
			{
				FieldInformation information = null;
				foreach (var info in Information.Fields)
				{
					if (info.Name == property.Name)
					{
						information = info;
						break;
					}
				}

				yield return new EditorField (information, property);
			}
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return ((IEnumerable<EditorField>)this).GetEnumerator ();
		}
	}
}
