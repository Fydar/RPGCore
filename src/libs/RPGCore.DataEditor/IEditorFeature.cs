namespace RPGCore.DataEditor;

/// <summary>
/// Allows for extension of an <see cref="IEditorToken"/>.
/// </summary>
public interface IEditorFeature
{
	/// <summary>
	/// Informs this <see cref="IEditorFeature"/> that it is being associated with an <see cref="IEditorToken"/>.
	/// </summary>
	/// <param name="token">The <see cref="IEditorToken"/> that is being associated with this <see cref="IEditorFeature"/>.</param>
	void AttachToToken(IEditorToken token);

	/// <summary>
	/// Informs this <see cref="IEditorFeature"/> that it is no longer associated with an <see cref="IEditorToken"/>.
	/// </summary>
	/// <param name="token">The <see cref="IEditorToken"/> that is no longer being associated with this <see cref="IEditorFeature"/>.</param>
	void RemoveFromToken(IEditorToken token);
}
