using RPGCore.DataEditor.Manifest;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace RPGCore.DataEditor;

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

	/// <summary>
	/// A collection of <see cref="IEditorFeature"/>s associated with this <see cref="EditorNull"/>.
	/// </summary>
	public FeatureCollection<EditorNull> Features { get; }

	/// <inheritdoc/>
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	FeatureCollection IEditorToken.Features => Features;

	/// <inheritdoc/>
	public TypeName? Type => null;

	/// <inheritdoc/>
	public event PropertyChangedEventHandler PropertyChanged;

	internal EditorNull(EditorSession session)
	{
		Session = session;
		comments = new List<string>();
		Features = new FeatureCollection<EditorNull>(this);

		PropertyChanged = delegate { };
	}

	/// <inheritdoc/>
	public IEditorValue Duplicate()
	{
		return new EditorNull(Session);
	}
}
