using Newtonsoft.Json.Linq;
using RPGCore.DataEditor.Manifest;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RPGCore.DataEditor
{
	public class EditorObject : IEditorValue
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] public EditorSession Session { get; }

		public TypeInformation Type { get; }
		public Dictionary<string, EditorField> Fields { get; }

		private JToken json;

		public EditorObject(EditorSession session, TypeInformation type, JToken json)
		{
			Session = session;
			Type = type;
			this.json = json;

			Fields = new Dictionary<string, EditorField>();

			UpdateFields();
		}

		public void PopulateObject(object obj)
		{
			var newValue = obj == null
				? JValue.CreateNull()
				: (JToken)JObject.FromObject(obj);

			json.Replace(newValue);
			json = newValue;

			UpdateFields();
			Session.InvokeOnChanged();
		}

		private void UpdateFields()
		{
			Fields.Clear();
			if (json != null && json.Type == JTokenType.Object)
			{
				PopulateMissing((JObject)this.json, Type);

				foreach (var field in Type.Fields)
				{
					var propertyToken = this.json[field.Key].Parent;
					var property = (JProperty)propertyToken;

					Fields.Add(field.Key, new EditorField(Session, field.Value, property));
				}
			}
		}

		private static void PopulateMissing(JObject serialized, TypeInformation information)
		{
			// Remove any additional fields.
			foreach (var item in serialized.Children<JProperty>().ToList())
			{
				if (!information.Fields.ContainsKey(item.Name))
				{
					item.Remove();
				}
			}

			// Populate missing fields with default values.
			if (information.Fields != null)
			{
				foreach (var field in information.Fields)
				{
					if (!serialized.ContainsKey(field.Key))
					{
						serialized.Add(field.Key, field.Value.DefaultValue);
					}
				}
			}
		}
	}
}
