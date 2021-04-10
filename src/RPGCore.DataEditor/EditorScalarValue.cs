using RPGCore.DataEditor.Manifest;
using System.Diagnostics;

namespace RPGCore.DataEditor
{
	/// <summary>
	/// An editable value.
	/// </summary>
	public class EditorScalarValue : IEditorValue
	{
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
			Type = type;
		}
	}
}
