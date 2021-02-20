using System;

namespace RPGCore.Packages
{
	/// <summary>
	/// <para>Used for exploring the content of a package.</para>
	/// </summary>
	public interface IExplorer : IDisposable
	{
		/// <summary>
		/// <para>The project definition for this package.</para>
		/// </summary>
		IDefinition Definition { get; }

		/// <summary>
		/// <para>A collection of all of the resources contained in this package.</para>
		/// </summary>
		IResourceCollection Resources { get; }

		/// <summary>
		/// <para>An index of the tags contained within this package for performing asset queries.</para>
		/// </summary>
		ITagsCollection Tags { get; }

		/// <summary>
		/// <para>A directory representing the root of the package.</para>
		/// </summary>
		IDirectory RootDirectory { get; }
	}
}
