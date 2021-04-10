using Newtonsoft.Json;
using System.Diagnostics;

namespace RPGCore.Packages
{
	/// <summary>
	/// Represents a configuration definition for the package.
	/// </summary>
	public class PackageDefinition : IDefinition
	{
		/// <summary>
		/// General properties defined for this package.
		/// </summary>
		public PackageDefinitionProperties Properties { get; set; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)] IDefinitionProperties IDefinition.Properties => Properties;

		/// <summary>
		/// Initialises a new instance of a <see cref="PackageDefinition"/>.
		/// </summary>
		/// <param name="properties">Properties usedd by the definition.</param>
		[JsonConstructor]
		public PackageDefinition(PackageDefinitionProperties properties)
		{
			Properties = properties;
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return $"{Properties.Name}, Version={Properties.Version}";
		}
	}
}
