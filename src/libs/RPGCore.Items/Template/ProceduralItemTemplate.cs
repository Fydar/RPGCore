using RPGCore.DataEditor.CSharp;

namespace RPGCore.Items
{
	[EditorType]
	public class ProceduralItemTemplate : ItemTemplate
	{
		/// <inheritdoc/>
		public override string ToString()
		{
			return $"{nameof(ProceduralItemTemplate)}({DisplayName})";
		}
	}
}
