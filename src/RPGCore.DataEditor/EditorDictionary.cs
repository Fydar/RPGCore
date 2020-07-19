using Newtonsoft.Json.Linq;
using RPGCore.DataEditor.Manifest;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.DataEditor
{
	public class EditorDictionary : IEditorValue
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] public EditorSession Session { get; }

		public Dictionary<string, EditorKeyValuePair> KeyValuePairs { get; }
		public TypeInformation ValueType { get; }

		private JToken json;

		public EditorDictionary(EditorSession session, TypeInformation valueType, JToken json)
		{
			Session = session;
			ValueType = valueType;

			this.json = json;

			KeyValuePairs = new Dictionary<string, EditorKeyValuePair>();

			if (json.Type != JTokenType.Null)
			{
				var jsonObject = (JObject)json;
				foreach (var kvp in jsonObject.Properties())
				{
					var value = new EditorKeyValuePair(Session, ValueType, kvp);
					KeyValuePairs.Add(kvp.Name, value);
				}
			}
		}

		public bool Remove(string key)
		{
			if (json.Type == JTokenType.Null)
			{
				return false;
			}

			var jsonObject = (JObject)json;
			bool removed = jsonObject.Remove(key);

			if (!removed)
			{
				return false;
			}

			KeyValuePairs.Remove(key);

			return true;
		}

		public void Add(string key)
		{
			var duplicate = ValueType.DefaultValue?.DeepClone() ?? JValue.CreateNull();

			JObject jsonObject;
			if (json.Type == JTokenType.Null)
			{
				jsonObject = new JObject();
				json.Replace(jsonObject);
				json = jsonObject;
			}
			else
			{
				jsonObject = (JObject)json;
			}

			jsonObject.Add(key, duplicate);

			if (!KeyValuePairs.TryGetValue(key, out var field))
			{
				field = new EditorKeyValuePair(Session, ValueType, (JProperty)jsonObject[key]);
			}
			KeyValuePairs.Add(key, field);
			Session.InvokeOnChanged();
		}
	}
}
