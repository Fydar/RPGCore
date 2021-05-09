namespace RPGCore.DataEditor
{
	public interface IFileSaver
	{
		void Save(EditorSession editorSession, IEditorValue save);
	}
}
