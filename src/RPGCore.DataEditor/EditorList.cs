using Newtonsoft.Json.Linq;
using RPGCore.DataEditor.Manifest;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.DataEditor
{
	public class EditorList : IEditorValue
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] public EditorSession Session { get; }

		public TypeInformation ElementType { get; }
		public List<IEditorValue> Elements { get; }

		private JToken json;

		public EditorList(EditorSession session, TypeInformation elementType, JToken json)
		{
			Session = session;
			ElementType = elementType;
			this.json = json;

			Elements = new List<IEditorValue>();

			foreach (var element in json)
			{
				Elements.Add(Session.CreateValue(elementType, null, element));
			}
		}

		public void SetArraySize(int size)
		{
			JArray jsonArray;
			if (json.Type == JTokenType.Null)
			{
				jsonArray = new JArray();
				json.Replace(jsonArray);
				json = jsonArray;
			}
			else
			{
				jsonArray = (JArray)json;
			}

			while (jsonArray.Count > size)
			{
				int removeIndex = jsonArray.Count - 1;

				jsonArray.RemoveAt(removeIndex);
				Elements.RemoveAt(removeIndex);
			}

			while (jsonArray.Count < size)
			{
				var duplicate = ElementType.DefaultValue?.DeepClone() ?? JValue.CreateNull();
				jsonArray.Add(duplicate);
				Elements.Add(new EditorValue(Session, ElementType, duplicate));
			}
			Session.InvokeOnChanged();
		}
	}
}
