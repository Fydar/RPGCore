namespace RPGCore.Packages
{
	/// <summary>
	/// General properties for a package.
	/// </summary>
	public interface IDefinitionProperties
	{
		/// <summary>
		/// The name of the package.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// The version of the package.
		/// </summary>
		string Version { get; }
	}
}
