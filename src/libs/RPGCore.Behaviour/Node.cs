using RPGCore.Data.Polymorphic.Inline;
using System.Text.Json.Serialization;

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
	[JsonPropertyOrder(-1000)]
	public string Id { get; set; } = string.Empty;

	/// <summary>
	/// Creates a <see cref="GraphDefinitionNode"/> from this <see cref="Node"/>.
	/// </summary>
	/// <returns></returns>
	public abstract NodeDefinition CreateDefinition();
}
