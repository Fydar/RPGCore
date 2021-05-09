using RPGCore.DataEditor.Manifest;

namespace RPGCore.DataEditor
{
	public class TypeDefaultLoader : IFileLoader
	{
		/// <inheritdoc/>
		public IEditorValue Load(EditorSession editorSession, TypeName? type)
		{
			return editorSession.CreateInstatedValue(type);
		}
	}
}
