using RPGCore.DataEditor.Files;
using RPGCore.DataEditor.Manifest;
using System.Text;

namespace RPGCore.DataEditor.UnitTests.Utility
{
	public class StringContentLoader : IFileLoader
	{
		private readonly byte[] value;

		public StringContentLoader(string value)
		{
			this.value = Encoding.UTF8.GetBytes(value);
		}

		/// <inheritdoc/>
		public IEditorValue Load(EditorSession editorSession, TypeName type)
		{
			return editorSession.Serializer.DeserializeValue(editorSession, type, value);
		}
	}
}
