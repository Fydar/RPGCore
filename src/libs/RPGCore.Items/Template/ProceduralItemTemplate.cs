using RPGCore.Data;

namespace RPGCore.Items;

[EditableType]
public class ProceduralItemTemplate : ItemTemplate
{
	/// <inheritdoc/>
	public override string ToString()
	{
		return $"{nameof(ProceduralItemTemplate)}({DisplayName})";
	}
}
