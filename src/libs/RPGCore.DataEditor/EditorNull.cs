using System.Diagnostics;

namespace RPGCore.DataEditor
{
	/// <summary>
	/// An editor value representing null.
	/// </summary>
	public class EditorNull : IEditorValue
	{
		/// <inheritdoc/>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public EditorSession Session { get; }

		internal EditorNull(EditorSession session)
		{
			Session = session;
		}
	}
}
