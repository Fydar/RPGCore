using Newtonsoft.Json.Linq;
using RPGCore.DataEditor.Manifest;
using System.Diagnostics;

namespace RPGCore.DataEditor
{
	public class EditorKeyValuePair
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)] public EditorSession Session { get; }
		public TypeInformation ValueType { get; }

		public IEditorValue Value { get; }

		private readonly JProperty json;

		public EditorKeyValuePair(EditorSession session, TypeInformation valueType, JProperty json)
		{
			Session = session;
			ValueType = valueType;

			this.json = json;

			Value = session.CreateValue(valueType, null, json.Value);
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return $"{{{json.Name}: {json.Value}}}";
		}
	}
}
