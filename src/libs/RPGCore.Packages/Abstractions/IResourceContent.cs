using System.IO;

namespace RPGCore.Packages
{
	/// <summary>
	/// Represents the content of a resource in a package.
	/// </summary>
	public interface IResourceContent
	{
		/// <summary>
		/// The uncompressed size of the resource.
		/// </summary>
		long UncompressedSize { get; }

		/// <summary>
		/// Opens the resource for reading.
		/// </summary>
		/// <returns>A <see cref="Stream"/> representing the content of the resource.</returns>
		Stream OpenRead();
	}
}
