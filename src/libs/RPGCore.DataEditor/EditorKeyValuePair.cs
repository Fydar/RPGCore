using System.Collections.Generic;
using System.ComponentModel;
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
		public EditorReplaceable Key { get; }

		/// <summary>
		/// Gets the value in the key/value pair.
		/// </summary>
		public EditorReplaceable Value { get; }

		/// <summary>
		/// A collection of <see cref="IEditorFeature"/>s associated with this <see cref="EditorKeyValuePair"/>.
		/// </summary>
		public FeatureCollection<EditorKeyValuePair> Features { get; }

		/// <inheritdoc/>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		FeatureCollection IEditorToken.Features => Features;

		/// <inheritdoc/>
		public event PropertyChangedEventHandler PropertyChanged;

		internal EditorKeyValuePair(EditorDictionary parent, IEditorValue key, IEditorValue value)
		{
			Parent = parent;
			Key = new EditorReplaceable(parent.Session, parent.KeyType, key);
			Value = new EditorReplaceable(parent.Session, parent.ValueType, value);
			Features = new FeatureCollection<EditorKeyValuePair>(this);

			comments = new List<string>();

			PropertyChanged = delegate { };
		}
	}
}
