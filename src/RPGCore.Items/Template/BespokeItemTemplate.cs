using RPGCore.Behaviour;
using RPGCore.DataEditor.CSharp;

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
