using RPGCore.Behaviour.Manifest;

namespace RPGCore.Items
{
	[EditorType]
	public class ProceduralItemTemplate : ItemTemplate
	{
		public override string ToString()
		{
			return $"{nameof(ProceduralItemTemplate)}({DisplayName})";
		}
	}
}
