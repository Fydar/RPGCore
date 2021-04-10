using System;

namespace RPGCore.DataEditor.Manifest
{
	/// <summary>
	/// A qualified type template.
	/// </summary>
	public sealed class SchemaQualifiedType
	{
		/// <summary>
		/// A string identifier that is used to identifiy the type template.
		/// </summary>
		public string Identifier { get; set; } = string.Empty;

		/// <summary>
		/// Templated types that make up this type.
		/// </summary>
		public SchemaQualifiedType[] TemplateTypes { get; set; } = Array.Empty<SchemaQualifiedType>();

		/// <summary>
		/// Determines whether the type is nullable.
		/// </summary>
		public bool IsNullable { get; set; }

		/// <summary>
		/// Initialises a new instance of <see cref="SchemaQualifiedType"/>.
		/// </summary>
		/// <param name="identifier">An identifier for the type.</param>
		public SchemaQualifiedType(string identifier)
		{
			Identifier = identifier;
		}

		/// <summary>
		/// Initialises a new instance of <see cref="SchemaQualifiedType"/>.
		/// </summary>
		/// <param name="identifier">An identifier for the type.</param>
		/// <param name="templateTypes">Parameters that qualify the templated type.</param>
		public SchemaQualifiedType(string identifier, SchemaQualifiedType[] templateTypes)
		{
			Identifier = identifier;
			TemplateTypes = templateTypes;
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			if (TemplateTypes.Length == 0)
			{
				return $"{Identifier}{(IsNullable ? "?" : "")}";
			}
			return $"{Identifier}<{string.Join(", ", (object[])TemplateTypes)}>{(IsNullable ? "?" : "")}";
		}
	}
}
