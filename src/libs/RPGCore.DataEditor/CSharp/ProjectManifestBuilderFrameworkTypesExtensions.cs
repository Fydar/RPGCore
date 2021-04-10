using RPGCore.DataEditor.CSharp;

namespace RPGCore.DataEditor.Manifest
{
	/// <summary>
	/// A collection of extensions for adding support for .NET framework types.
	/// </summary>
	public static class ProjectManifestBuilderFrameworkTypesExtensions
	{
		/// <summary>
		/// Adds types to the project manifest for all C# types.
		/// </summary>
		/// <param name="builder">The <see cref="ProjectManifestBuilder"/> to add types to.</param>
		/// <returns>Returns the current instance of the <see cref="ProjectManifestBuilder"/>.</returns>
		public static ProjectManifestBuilder UseFrameworkTypes(this ProjectManifestBuilder builder)
		{
			foreach (var type in BuiltInTypes.frameworkTypes)
			{
				builder.AddType(BuiltInTypes.Construct(type));
			}
			return builder;
		}
	}
}
