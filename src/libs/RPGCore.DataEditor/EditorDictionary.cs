using RPGCore.DataEditor.Manifest;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.DataEditor
{
	/// <summary>
	/// An editable data structure that utilises a collection of key-value pairs.
	/// </summary>
	public class EditorDictionary : IEditorValue
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly List<string> comments;

		/// <inheritdoc/>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public EditorSession Session { get; }

		/// <inheritdoc/>
		public IList<string> Comments => comments;

		/// <summary>
		/// The type of the dictionary keys.
		/// </summary>
		public SchemaQualifiedType KeyType { get; }

		/// <summary>
		/// the type of the dictionary values.
		/// </summary>
		public SchemaQualifiedType ValueType { get; }

		/// <summary>
		/// All key-value pairs contained within this dictionary.
		/// </summary>
		public List<EditorKeyValuePair> KeyValuePairs { get; }

		internal EditorDictionary(EditorSession session, SchemaQualifiedType keyType, SchemaQualifiedType valueType)
		{
			Session = session;
			KeyType = keyType;
			ValueType = valueType;

			KeyValuePairs = new List<EditorKeyValuePair>();
			comments = new List<string>();
		}

		/// <summary>
		/// Removes a value from the <see cref="EditorDictionary"/>.
		/// </summary>
		public bool Remove(EditorKeyValuePair kvp)
		{
			return KeyValuePairs.Remove(kvp);
		}

		/// <summary>
		/// Adds a new value to the <see cref="EditorDictionary"/>.
		/// </summary>
		public void Add()
		{
			KeyValuePairs.Add(new EditorKeyValuePair(
				this,
				Session.CreateDefaultValue(KeyType),
				Session.CreateDefaultValue(ValueType)));
		}
	}
}
