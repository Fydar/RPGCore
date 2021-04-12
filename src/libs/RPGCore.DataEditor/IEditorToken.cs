using System.Collections.Generic;

namespace RPGCore.DataEditor
{
	/// <summary>
	/// Represents a token in an editable hierarchy.
	/// </summary>
	public interface IEditorToken
	{
		/// <summary>
		/// The <see cref="EditorSession"/> that this <see cref="IEditorToken"/> is belongs to.
		/// </summary>
		EditorSession Session { get; }

		/// <summary>
		/// All comments associated with this editor token.
		/// </summary>
		IList<string> Comments { get; }
	}
}
