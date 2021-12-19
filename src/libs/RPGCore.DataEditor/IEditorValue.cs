using RPGCore.DataEditor.Manifest;

namespace RPGCore.DataEditor
{
	/// <summary>
	/// Represents and editable value.
	/// </summary>
	public interface IEditorValue : IEditorToken
	{
		/// <summary>
		/// The type that this value can be described with.
		/// </summary>
		TypeName? Type { get; }

		/// <summary>
		/// Duplicates this <see cref="IEditorValue"/>.
		/// </summary>
		/// <returns>A duplicate of this <see cref="IEditorValue"/>.</returns>
		IEditorValue Duplicate();
	}
}
