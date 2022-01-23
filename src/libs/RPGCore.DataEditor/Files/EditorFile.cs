using RPGCore.DataEditor.Manifest;
using System;
using System.Diagnostics;

namespace RPGCore.DataEditor.Files;

/// <summary>
/// A root data structure for data editing.
/// </summary>
public class EditorFile
{
	/// <summary>
	/// An <see cref="Session"/> that provides editor configuration and state.
	/// </summary>
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public EditorSession Session { get; }

	/// <summary>
	/// An <see cref="IEditorValue"/> representing the root of the <see cref="EditorFile"/>.
	/// </summary>
	public IEditorValue Root { get; set; }

	private readonly IFileLoader? fileLoader;
	private readonly IFileSaver? fileSaver;
	private readonly TypeName type;

	internal EditorFile(EditorSession session, IFileLoader? fileLoader, IFileSaver? fileSaver, TypeName type)
	{
		Session = session;
		this.fileLoader = fileLoader;
		this.fileSaver = fileSaver;
		this.type = type;

		if (fileLoader == null)
		{
			if (type.IsUnknown)
			{
				throw new InvalidOperationException("Cannot use an unknown type when no loader is supplied.");
			}
			else
			{
				Root = session.CreateInstatedValue(type);
			}
		}
		else
		{
			Root = fileLoader.Load(Session, type);
		}
	}

	public void Save()
	{
		if (fileSaver == null)
		{
			throw new InvalidOperationException("Cannot save file as it has no destination to save to.");
		}

		fileSaver.Save(Session, type, Root);
	}

	public void Reload()
	{
		if (fileLoader == null)
		{
			throw new InvalidOperationException("Cannot reload file as it has no source to load from.");
		}

		Root = fileLoader.Load(Session, type);
	}
}
