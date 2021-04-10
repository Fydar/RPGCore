namespace RPGCore.DataEditor.Manifest
{
	/// <summary>
	/// Information for a field.
	/// </summary>
	public sealed class SchemaField
	{
		/// <summary>
		/// The name of this field.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// A description of this fields usage.
		/// </summary>
		public string Description { get; }

		/// <summary>
		/// The type of the value contained within the field.
		/// </summary>
		public SchemaQualifiedType Type { get; }

		/// <summary>
		/// The default value of this field.
		/// </summary>
		public string? InstatedValue { get; }

		internal SchemaField(string name, string description, SchemaQualifiedType type, string? instatedValue)
		{
			Name = name;
			Description = description;
			Type = type;
			InstatedValue = string.Empty;
			InstatedValue = instatedValue;
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return $"{Type} {Name}";
		}
	}
}
