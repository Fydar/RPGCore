using RPGCore.DataEditor.Manifest;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.DataEditor
{
	/// <summary>
	/// An editable value.
	/// </summary>
	public class EditorScalarValue : IEditorValue
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly List<string> comments;

		/// <inheritdoc/>
		public IList<string> Comments => comments;

		/// <inheritdoc/>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public EditorSession Session { get; }

		/// <summary>
		/// The type of this <see cref="EditorScalarValue"/>.
		/// </summary>
		public SchemaQualifiedType Type { get; }

		/// <summary>
		/// An <see cref="object"/> representing the value of this <see cref="EditorScalarValue"/>.
		/// </summary>
		public object? Value { get; set; }

		internal EditorScalarValue(EditorSession session, SchemaQualifiedType type)
		{
			Session = session;
			comments = new List<string>();

			Type = type;
		}

		internal EditorScalarValue(EditorSession session, SchemaQualifiedType type, object? value)
			: this(session, type)
		{
			Value = value;
		}
	}
}
