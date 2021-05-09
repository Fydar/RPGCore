using System.IO;

namespace RPGCore.DataEditor.UnitTests.Utility
{
	public static class EditorHierarchyDrawer
	{
		public static void Write(TextWriter writer, IEditorValue editorValue)
		{
			switch (editorValue)
			{
				case EditorObject editorObject:

					writer.WriteLine("{");
					foreach (var field in editorObject.Fields)
					{
						writer.WriteLine(field);
					}
					writer.WriteLine("}");

					break;
			}
		}
	}
}
