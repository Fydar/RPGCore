using RPGCore.DataEditor.Manifest;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.DataEditor
{
	/// <summary>
	/// An editable data structure with indexed elements.
	/// </summary>
	public class EditorList : IEditorValue
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly List<string> comments;

		/// <inheritdoc/>
		public IList<string> Comments => comments;

		/// <inheritdoc/>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public EditorSession Session { get; }

		public SchemaQualifiedType ElementType { get; }
		public List<IEditorValue> Elements { get; }

		internal EditorList(EditorSession session, SchemaQualifiedType elementType)
		{
			Session = session;
			ElementType = elementType;

			Elements = new List<IEditorValue>();
			comments = new List<string>();
		}

		/// <summary>
		/// Sets the size of the array; creates new array elements to match the new size of the array.
		/// </summary>
		/// <param name="size"></param>
		public void SetArraySize(int size)
		{
			while (Elements.Count > size)
			{
				Elements.RemoveAt(Elements.Count - 1);
			}
			while (Elements.Count < size)
			{
				Elements.Add(Session.CreateDefaultValue(ElementType));
			}
		}
	}
}
