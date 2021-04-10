namespace RPGCore.DataEditor
{
	/// <summary>
	/// Represents and editable value.
	/// </summary>
	public interface IEditorValue
	{
		/// <summary>
		/// The <see cref="EditorSession"/> that this <see cref="IEditorValue"/> is belongs to.
		/// </summary>
		EditorSession Session { get; }
	}
}
