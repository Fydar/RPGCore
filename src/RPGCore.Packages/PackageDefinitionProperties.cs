using Newtonsoft.Json;

namespace RPGCore.Packages
{
	/// <summary>
	/// General properties for a package.
	/// </summary>
	public class PackageDefinitionProperties : IDefinitionProperties
	{
		/// <inheritdoc/>
		public string Name { get; internal set; }

		/// <inheritdoc/>
		public string Version { get; internal set; }

		/// <summary>
		/// Initialises a new instance of a <see cref="PackageDefinitionProperties"/>.
		/// </summary>
		public PackageDefinitionProperties()
		{
		}

		/// <summary>
		/// Initialises a new instance of a <see cref="PackageDefinitionProperties"/>.
		/// </summary>
		/// <param name="name">A name for the package.</param>
		/// <param name="version">A version for the package.</param>
		[JsonConstructor]
		public PackageDefinitionProperties(string name, string version)
		{
			Name = name;
			Version = version;
		}
	}
}
