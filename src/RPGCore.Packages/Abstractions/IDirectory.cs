namespace RPGCore.Packages
{
	/// <summary>
	/// <para>Represents a <see cref="IDirectory"/> loaded from an <see cref="IExplorer"/>.</para>
	/// </summary>
	public interface IDirectory
	{
		/// <summary>
		/// <para>The short name of the resource.</para>
		/// </summary>
		string Name { get; }

		/// <summary>
		/// <para>The full name of the <see cref="IDirectory"/> within the <see cref="IExplorer"/>.</para>
		/// </summary>
		string FullName { get; }

		/// <summary>
		/// <para>The <see cref="IDirectory"/> that this <see cref="IDirectory"/> is within.</para>
		/// </summary>
		IDirectory? Parent { get; }

		/// <summary>
		/// <para>The <see cref="IDirectory"/>s contained within this <see cref="IDirectory"/>.</para>
		/// </summary>
		IDirectoryCollection Directories { get; }

		/// <summary>
		/// <para>The <see cref="IResource"/>s contained within this <see cref="IDirectory"/>.</para>
		/// </summary>
		IResourceCollection Resources { get; }
	}
}
