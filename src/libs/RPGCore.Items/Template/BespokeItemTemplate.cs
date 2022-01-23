using RPGCore.Data;

namespace RPGCore.Items;

[EditableType]
public class BespokeItemTemplate : ItemTemplate
{
	/// <inheritdoc/>
	public override string ToString()
	{
		return $"{nameof(BespokeItemTemplate)}({DisplayName})";
	}
}
