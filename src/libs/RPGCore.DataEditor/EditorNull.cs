using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.DataEditor
{
	/// <summary>
	/// An editor value representing null.
	/// </summary>
	public class EditorNull : IEditorValue
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly List<string> comments;

		/// <inheritdoc/>
		public IList<string> Comments => comments;

		/// <inheritdoc/>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public EditorSession Session { get; }

		internal EditorNull(EditorSession session)
		{
			Session = session;
			comments = new List<string>();
		}
	}
}
