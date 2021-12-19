using RPGCore.DataEditor.Manifest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

		/// <inheritdoc/>
		public TypeName Type { get; }

		/// <summary>
		/// All key-value pairs contained within this dictionary.
		/// </summary>
		public List<EditorKeyValuePair> KeyValuePairs { get; }

		/// <summary>
		/// A collection of <see cref="IEditorFeature"/>s associated with this <see cref="EditorDictionary"/>.
		/// </summary>
		public FeatureCollection<EditorDictionary> Features { get; }

		/// <summary>
		/// The type of the dictionary keys.
		/// </summary>
		public TypeName KeyType => Type.TemplateTypes[0];

		/// <summary>
		/// the type of the dictionary values.
		/// </summary>
		public TypeName ValueType => Type.TemplateTypes[1];

		/// <inheritdoc/>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		FeatureCollection IEditorToken.Features => Features;

		/// <summary>
		/// The length of this <see cref="EditorDictionary"/>.
		/// </summary>
		public int Length
		{
			get => KeyValuePairs.Count;
			set => SetLength(value);
		}

		/// <inheritdoc/>
		public event PropertyChangedEventHandler PropertyChanged;

		internal EditorDictionary(EditorSession session, TypeName type)
		{
			Session = session;
			Type = type;
			Features = new FeatureCollection<EditorDictionary>(this);

			KeyValuePairs = new List<EditorKeyValuePair>();
			comments = new List<string>();

			PropertyChanged = delegate { };
		}

		/// <summary>
		/// Sets the size of the dictionary; creates new key-value pairs to match the new size.
		/// </summary>
		/// <param name="size">The new size of this <see cref="EditorList"/>.</param>
		/// <param name="insertNulls">If the <see cref="ValueType"/> of this <see cref="EditorDictionary"/> allows <c>null</c> values, insert nulls in new key-value pairs.</param>
		public void SetLength(int size, bool insertNulls = true)
		{
			if (size < 0)
			{
				throw new ArgumentOutOfRangeException("Cannot set dictionary size to a value that is smaller than 0.", nameof(size));
			}

			bool modified = false;
			while (KeyValuePairs.Count > size)
			{
				KeyValuePairs.RemoveAt(KeyValuePairs.Count - 1);
				modified = true;
			}
			while (KeyValuePairs.Count < size)
			{
				if (insertNulls)
				{
					KeyValuePairs.Add(new EditorKeyValuePair(
						this,
						Session.CreateDefaultValue(KeyType),
						Session.CreateDefaultValue(ValueType)));
				}
				else
				{
					KeyValuePairs.Add(new EditorKeyValuePair(
						this,
						Session.CreateDefaultValue(KeyType),
						Session.CreateInstatedValue(ValueType)));
				}

				modified = true;
			}
			if (modified)
			{
				InvokeOnSizeChanged();
			}
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
		public void Add(bool insertNulls = false)
		{
			KeyValuePairs.Add(new EditorKeyValuePair(
				this,
				Session.CreateDefaultValue(KeyType),
				insertNulls ? Session.CreateDefaultValue(ValueType) : Session.CreateInstatedValue(ValueType)));

			InvokeOnSizeChanged();
		}

		/// <summary>
		/// Adds a new value to the <see cref="EditorDictionary"/>.
		/// </summary>
		public void Add(IEditorValue key, IEditorValue value)
		{
			KeyValuePairs.Add(new EditorKeyValuePair(
				this,
				key,
				value));

			InvokeOnSizeChanged();
		}

		/// <inheritdoc/>
		public IEditorValue Duplicate()
		{
			var editorDictionary = new EditorDictionary(Session, Type);

			foreach (var keyValuePair in KeyValuePairs)
			{
				editorDictionary.Add(
					keyValuePair.Key.Value.Duplicate(),
					keyValuePair.Value.Value.Duplicate());
			}

			return editorDictionary;
		}

		private void InvokeOnSizeChanged()
		{
			PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(KeyValuePairs)));
			PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Length)));
		}
	}
}
