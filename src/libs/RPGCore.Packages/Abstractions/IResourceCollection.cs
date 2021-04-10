using System.Collections.Generic;

namespace RPGCore.Packages
{
	/// <summary>
	/// <para>Represents a collection of <see cref="IResource"/>.</para>
	/// </summary>
	public interface IResourceCollection : IEnumerable<IResource>
	{
		/// <summary>
		/// <para>The quantity of resource contained within the collection.</para>
		/// </summary>
		int Count { get; }

		/// <summary>
		/// <para>Retrieves a resource from the collection by a string-key.</para>
		/// </summary>
		/// <param name="key">The string-key for the resource in the collection.</param>
		/// <returns>The <see cref="IResource"/> found within the collection; otherwise returns <c>null</c>.</returns>
		IResource? this[string key] { get; }

		/// <summary>
		/// <para>Determines whether a resource with the key is contained within the collection.</para>
		/// </summary>
		/// <param name="key"></param>
		/// <returns>Returns <c>true</c> if a resource with the specified key is contained within the collection;
		/// otherwise returns <c>false</c>.</returns>
		bool Contains(string key);

		/// <summary>
		/// <para>Attempts to find a resource with a string key.</para>
		/// </summary>
		/// <param name="key">The string-key for the resource in the collection.</param>
		/// <param name="value">The found resource from the collection.</param>
		/// <returns>Returns <c>true</c> if a resource with the specified key is contained within the collection;
		/// otherwise returns <c>false</c>.</returns>
		bool TryGetResource(string key, out IResource value);
	}
}
