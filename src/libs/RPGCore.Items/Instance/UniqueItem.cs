namespace RPGCore.Items;

/// <summary>
/// Unique items can have data stored on the single instance of the item.
/// </summary>
public class UniqueItem : IItem
{
	/// <inheritdoc/>
	public ItemTemplate Template { get; }

	public UniqueItem(ItemTemplate template)
	{
		Template = template;
	}

	/// <inheritdoc/>
	public IItem Duplicate()
	{
		return new UniqueItem(Template);
	}
}
