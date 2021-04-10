namespace RPGCore.Packages
{
	/// <summary>
	/// Represents a configuration definition for the package.
	/// </summary>
	public interface IDefinition
	{
		/// <summary>
		/// General properties defined for this package.
		/// </summary>
		IDefinitionProperties Properties { get; }
	}
}
