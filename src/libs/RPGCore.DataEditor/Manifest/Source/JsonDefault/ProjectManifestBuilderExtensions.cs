using System;

namespace RPGCore.DataEditor.Manifest.Source.JsonDefault;

/// <summary>
/// A collection of extensions for adding support for user .NET objects.
/// </summary>
public static class ProjectManifestBuilderExtensions
{
	/// <summary>
	/// Adds a <see cref="Type"/> as a type to the <see cref="ProjectManifestBuilder"/>.
	/// </summary>
	/// <param name="builder">The <see cref="ProjectManifestBuilder"/> to add types to.</param>
	/// <returns>Returns the current instance of the <see cref="ProjectManifestBuilder"/>.</returns>
	public static ProjectManifestBuilder UseJsonTypes(this ProjectManifestBuilder builder)
	{
		builder.AddType(new SchemaType()
		{
			Name = "number",
			InstatedValue = "0"
		});

		builder.AddType(new SchemaType()
		{
			Name = "string",
			InstatedValue = "\"\""
		});

		builder.AddType(new SchemaType()
		{
			Name = "boolean",
			InstatedValue = "false"
		});

		builder.AddType(new SchemaType()
		{
			Name = "null",
			InstatedValue = "null"
		});

		return builder;
	}
}
