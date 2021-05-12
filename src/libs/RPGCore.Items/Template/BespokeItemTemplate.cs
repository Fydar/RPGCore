using RPGCore.DataEditor.Manifest.Source.RuntimeSource;

namespace RPGCore.Items
{
	[EditorType]
	public class BespokeItemTemplate : ItemTemplate
	{
		/// <inheritdoc/>
		public override string ToString()
		{
			return $"{nameof(BespokeItemTemplate)}({DisplayName})";
		}
	}
}
