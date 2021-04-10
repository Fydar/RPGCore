using System.Reflection;

namespace RPGCore.DataEditor.Manifest
{
	/// <summary>
	/// A schema representing a conversion from one type to another.
	/// </summary>
	public class SchemaTypeConversion
	{
		/// <summary>
		/// The type to convert from.
		/// </summary>
		public string FromType { get; set; } = string.Empty;

		/// <summary>
		/// The type to convert to.
		/// </summary>
		public string ToType { get; set; } = string.Empty;

		/// <summary>
		/// A description of how the conversion is performed.
		/// </summary>
		public string Description { get; set; } = string.Empty;

		/// <summary>
		/// Constructs a new conversion from a method.
		/// </summary>
		/// <param name="method">The method used in the conversion.</param>
		/// <returns>A new <see cref="SchemaTypeConversion"/> representing the convert method.</returns>
		public static SchemaTypeConversion Construct(MethodInfo method)
		{
			var typeConversion = new SchemaTypeConversion
			{
				Description = method.Name,
				ToType = method.ReturnType.Name,
				FromType = method.DeclaringType.Name
			};

			return typeConversion;
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return $"{FromType} -> {ToType}";
		}
	}
}
