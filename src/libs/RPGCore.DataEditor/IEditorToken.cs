using System.Collections.Generic;
using System.ComponentModel;

namespace RPGCore.DataEditor;

/// <summary>
/// Represents a token in an editable hierarchy.
/// </summary>
public interface IEditorToken : INotifyPropertyChanged
{
	/// <summary>
	/// The <see cref="EditorSession"/> that this <see cref="IEditorToken"/> is belongs to.
	/// </summary>
	EditorSession Session { get; }

	/// <summary>
	/// All <see cref="string"/> comments associated with this <see cref="IEditorToken"/>.
	/// </summary>
	IList<string> Comments { get; }

	/// <summary>
	/// A collection of <see cref="IEditorFeature"/>s associated with this <see cref="IEditorToken"/>.
	/// </summary>
	FeatureCollection Features { get; }
}
