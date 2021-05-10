using RPGCore.DataEditor.Manifest;

namespace RPGCore.DataEditor
{
	public interface IFileSaver
	{
		void Save(EditorSession editorSession, TypeName type, IEditorValue save);
	}
}
