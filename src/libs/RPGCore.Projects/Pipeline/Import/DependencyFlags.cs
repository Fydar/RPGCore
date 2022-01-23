namespace RPGCore.Projects.Pipeline;

public enum DependencyFlags
{
	/// <summary>
	/// <para>Represents no dependency flags.</para>
	/// </summary>
	None = 0,

	/// <summary>
	/// <para>Indicates that this dependency is allowed to create a circular dependency.</para>
	/// </summary>
	AllowCircular = 1 << 0,

	/// <summary>
	/// <para>Indicates that this dependency is an optional dependency.</para>
	/// <para>If the dependency is missing then the dependency configuration is valid.</para>
	/// </summary>
	Optional = 1 << 1,
}
