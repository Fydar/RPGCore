using System.Collections.Generic;

namespace RPGCore.DataEditor.Manifest
{
	/// <summary>
	/// A schema representing an object type.
	/// </summary>
	public class SchemaType
	{
		/// <summary>
		/// The name of the type.
		/// </summary>
		public string Name { get; set; } = string.Empty;

		/// <summary>
		/// A description of the type.
		/// </summary>
		public string? Description { get; set; }

		/// <summary>
		/// All fields contained within this type.
		/// </summary>
		public List<SchemaField>? Fields { get; set; }

		/// <summary>
		/// The instated value for this type.
		/// </summary>
		public string? InstatedValue { get; set; }

		/// <inheritdoc/>
		public override string ToString()
		{
			return Name;
		}
	}
}
