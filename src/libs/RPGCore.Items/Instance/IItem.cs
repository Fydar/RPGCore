namespace RPGCore.Items;

public interface IItem
{
	ItemTemplate Template { get; }

	/// <summary>
	/// Creates a duplicate of this item.
	/// </summary>
	/// <returns>A duplicate of this item.</returns>
	IItem Duplicate();
}
