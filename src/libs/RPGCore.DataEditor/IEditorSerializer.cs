using RPGCore.DataEditor.Manifest;
using System;
using System.IO;

namespace RPGCore.DataEditor
{
	/// <summary>
	/// A serializer for reading and writing <see cref="IEditorValue"/>.
	/// </summary>
	public interface IEditorSerializer
	{
		/// <summary>
		/// Reads an <see cref="IEditorValue"/> of the specified <paramref name="type"/> from the <paramref name="input"/>.
		/// </summary>
		/// <param name="session">The <see cref="EditorFile"/> to read into.</param>
		/// <param name="type">The type of the object to deserialize into.</param>
		/// <param name="input">The data to read from.</param>
		IEditorValue DeserializeValue(EditorSession session, TypeName type, ReadOnlySpan<byte> input);

		/// <summary>
		/// Writes an <see cref="IEditorValue"/> to a <see cref="Stream"/>.
		/// </summary>
		/// <param name="value">The <see cref="IEditorValue"/> to write to the <see cref="Stream"/>.</param>
		/// <param name="output">The <see cref="Stream"/> to write the <see cref="IEditorValue"/> to.</param>
		void SerializeValue(IEditorValue value, Stream output);
	}
}
