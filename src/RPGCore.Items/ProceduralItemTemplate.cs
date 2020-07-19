using RPGCore.Behaviour;

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
