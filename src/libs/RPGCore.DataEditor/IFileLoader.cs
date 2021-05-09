using RPGCore.DataEditor.Manifest;

namespace RPGCore.DataEditor
{
	public interface IFileLoader
	{
		IEditorValue Load(EditorSession editorSession, TypeName type);
	}
}
