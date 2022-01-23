using RPGCore.DataEditor.Manifest;
using System.IO;

namespace RPGCore.DataEditor.Files;

public class FileSystemFile : IFileLoader, IFileSaver
{
	private readonly FileInfo file;

	public FileSystemFile(FileInfo file)
	{
		this.file = file;
	}

	/// <inheritdoc/>
	public IEditorValue Load(EditorSession editorSession, TypeName type)
	{
		byte[] data = File.ReadAllBytes(file.FullName);
		return editorSession.Serializer.DeserializeValue(editorSession, type, data);
	}

	/// <inheritdoc/>
	public void Save(EditorSession editorSession, TypeName type, IEditorValue save)
	{
		file.Delete();
		using var stream = file.OpenWrite();

		editorSession.Serializer.SerializeValue(save, type, stream);
	}
}
