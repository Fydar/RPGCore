using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.DataEditor
{
	/// <summary>
	/// An editable hard-typed key-value pair belonging to a dictionary.
	/// </summary>
	public class EditorKeyValuePair : IEditorToken
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly List<string> comments;

		/// <inheritdoc/>
		public IList<string> Comments => comments;

		/// <inheritdoc/>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public EditorSession Session => Parent.Session;

		/// <summary>
		/// The <see cref="EditorDictionary"/> that this <see cref="EditorKeyValuePair"/> belongs to.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public EditorDictionary Parent { get; }

		/// <summary>
		/// Gets the key in the key/value pair.
		/// </summary>
		public IEditorValue Key { get; }

		/// <summary>
		/// Gets the value in the key/value pair.
		/// </summary>
		public IEditorValue Value { get; }

		internal EditorKeyValuePair(EditorDictionary parent, IEditorValue key, IEditorValue value)
		{
			Parent = parent;
			Key = key;
			Value = value;

			comments = new List<string>();
		}
	}
}
