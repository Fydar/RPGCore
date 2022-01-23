namespace RPGCore.DataEditor.Manifest;

/// <summary>
/// Information for a field.
/// </summary>
public sealed class SchemaField
{
	/// <summary>
	/// The name of this field.
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	/// A description of this fields usage.
	/// </summary>
	public string? Description { get; set; }

	/// <summary>
	/// The type of the value contained within the field.
	/// </summary>
	public TypeName Type { get; set; }

	/// <summary>
	/// The default value of this field.
	/// </summary>
	public string? InstatedValue { get; set; }

	public SchemaField()
	{
		Name = string.Empty;
	}

	internal SchemaField(string name, string? description, TypeName type, string? instatedValue)
	{
		Name = name;
		Description = description;
		Type = type;
		InstatedValue = instatedValue;
	}

	/// <inheritdoc/>
	public override string ToString()
	{
		return $"{Type} {Name}";
	}
}
