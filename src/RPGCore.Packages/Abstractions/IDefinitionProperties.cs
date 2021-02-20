namespace RPGCore.Packages
{
	/// <summary>
	/// <para>General properties for a package.</para>
	/// </summary>
	public interface IDefinitionProperties
	{
		/// <summary>
		/// <para>The name of the package.</para>
		/// </summary>
		string Name { get; }

		/// <summary>
		/// <para>The version of the package.</para>
		/// </summary>
		string Version { get; }
	}
}
