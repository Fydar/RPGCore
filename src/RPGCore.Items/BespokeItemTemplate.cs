using RPGCore.Behaviour.Manifest;

namespace RPGCore.Items
{
	[EditorType]
	public class BespokeItemTemplate : ItemTemplate
	{
		public override string ToString()
		{
			return $"{nameof(BespokeItemTemplate)}({DisplayName})";
		}
	}
}
