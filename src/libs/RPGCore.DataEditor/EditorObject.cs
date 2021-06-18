using RPGCore.DataEditor.Manifest;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace RPGCore.DataEditor
{
	/// <summary>
	/// An editable data structure that uses hard-typed fields.
	/// </summary>
	public class EditorObject : IEditorValue
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly List<string> comments;

		/// <inheritdoc/>
		public IList<string> Comments => comments;

		/// <inheritdoc/>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public EditorSession Session { get; }

		/// <summary>
		/// The type of the current instance of the <see cref="EditorObject"/>.
		/// </summary>
		public TypeName Type { get; }

		/// <summary>
		/// All <see cref="EditorField"/> contained within this object.
		/// </summary>
		public EditorFieldCollection Fields { get; }

		/// <inheritdoc/>
		public event PropertyChangedEventHandler PropertyChanged;

		internal EditorObject(EditorSession session, TypeName type)
		{
			Session = session;
			Type = type;
			comments = new List<string>();
			Fields = new EditorFieldCollection(this);

			PropertyChanged = delegate { };
		}
	}
}
