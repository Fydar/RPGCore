using RPGCore.DataEditor.Manifest;

namespace RPGCore.DataEditor.Files
{
	public interface IFileLoader
	{
		IEditorValue Load(EditorSession editorSession, TypeName type);
	}
}
