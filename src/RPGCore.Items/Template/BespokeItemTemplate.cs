using RPGCore.Behaviour;

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
