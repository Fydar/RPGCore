namespace RPGCore.Packages
{
	/// <summary>
	/// <para>Represents a resource loaded from an <see cref="IExplorer"/>.</para>
	/// </summary>
	public interface IResource
	{
		/// <summary>
		/// <para>The short name of the resource.</para>
		/// </summary>
		string Name { get; }

		/// <summary>
		/// <para>The full name of the resource within the <see cref="IExplorer"/>.</para>
		/// </summary>
		string FullName { get; }

		/// <summary>
		/// <para>The file extension that the resource name ends with.</para>
		/// </summary>
		string Extension { get; }

		/// <summary>
		/// <para>All of the tags associated with this <see cref="IResource"/>.</para>
		/// </summary>
		IResourceTags Tags { get; }

		/// <summary>
		/// <para>The <see cref="IDirectory"/> that this <see cref="IResource"/> is within.</para>
		/// </summary>
		IDirectory Directory { get; }

		/// <summary>
		/// <para>A collection representing all of the dependencies this <see cref="IResource"/> has on other <see cref="IResource"/>s.</para>
		/// </summary>
		IResourceDependencyCollection Dependencies { get; }

		/// <summary>
		/// <para>A collection representing all dependant <see cref="IResource"/>s.</para>
		/// </summary>
		IResourceDependencyCollection Dependants { get; }

		/// <summary>
		/// <para>The <see cref="IExplorer"/> that this <see cref="IResource"/> was loaded from.</para>
		/// </summary>
		IExplorer Explorer { get; }

		/// <summary>
		/// <para>The <see cref="IResourceContent"/> of this <see cref="IResource"/>.</para>
		/// </summary>
		IResourceContent Content { get; }
	}
}
