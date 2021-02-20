using System.Collections.Generic;

namespace RPGCore.Packages
{
	/// <summary>
	/// <para>A dependency of one <see cref="IResource"/> on another <see cref="IResource"/>.</para>
	/// </summary>
	public interface IResourceDependency
	{
		/// <summary>
		/// <para>Determines the validity of this dependency relationship.</para>
		/// </summary>
		bool IsValid { get; }

		/// <summary>
		/// <para>A key used to identify this dependency.</para>
		/// </summary>
		string Key { get; }

		/// <summary>
		/// <para>The resource referenced by this dependency.</para>
		/// </summary>
		IResource? Resource { get; }

		/// <summary>
		/// <para>The resource referenced by this dependency.</para>
		/// </summary>
		IReadOnlyDictionary<string, string> Metadata { get; }
	}
}
