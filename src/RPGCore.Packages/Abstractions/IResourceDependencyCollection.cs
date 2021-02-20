using System.Collections.Generic;

namespace RPGCore.Packages
{
	/// <summary>
	/// Represents a collection of <see cref="IResourceDependency"/>.
	/// </summary>
	public interface IResourceDependencyCollection : IEnumerable<IResourceDependency>
	{
		/// <summary>
		/// The quantity of dependencies contained within the collection.
		/// </summary>
		int Count { get; }

		/// <summary>
		/// Retrieves a <see cref="IResourceDependency"/> from the collection by an index.
		/// </summary>
		/// <param name="index">The integer-index for the resource in the collection.</param>
		/// <returns>The <see cref="IResourceDependency"/> found within the collection; otherwise returns <c>null</c>.</returns>
		IResourceDependency this[int index] { get; }
	}
}
