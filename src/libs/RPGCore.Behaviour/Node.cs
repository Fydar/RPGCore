using RPGCore.Data.Polymorphic.Inline;

namespace RPGCore.Behaviour;

/// <summary>
/// A base class for all nodes contained within a <see cref="Graph"/>.
/// </summary>
[SerializeBaseType(TypeName.Name)]
public abstract class Node
{
	/// <summary>
	/// An identifier for this <see cref="Node"/> contained within the <see cref="Graph"/>.
	/// </summary>
	public string Id { get; set; } = string.Empty;

	/// <summary>
	/// Creates a <see cref="NodeDefinition"/> from this <see cref="Node"/>.
	/// </summary>
	/// <returns></returns>
	public abstract NodeDefinition CreateDefinition();
}
