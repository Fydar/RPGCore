using RPGCore.DataEditor.Manifest.Source.RuntimeSource;
using System;
using System.Text.Json;

namespace RPGCore.DataEditor.Manifest.Source.JsonSerializer;

/// <summary>
/// A collection of extensions for adding support for user .NET objects.
/// </summary>
public static class ProjectManifestBuilderExtensions
{
	/// <summary>
	/// Adds a <see cref="Type"/> as a type to the <see cref="ProjectManifestBuilder"/>.
	/// </summary>
	/// <param name="builder">The <see cref="ProjectManifestBuilder"/> to add types to.</param>
	/// <param name="jsonSerializerOptions"></param>
	/// <param name="options"></param>
	/// <returns>Returns the current instance of the <see cref="ProjectManifestBuilder"/>.</returns>
	public static ProjectManifestBuilder UseTypesFromJsonSerializer(this ProjectManifestBuilder builder, JsonSerializerOptions jsonSerializerOptions, Action<RuntimeProjectManifestBuilderOptions> options)
	{
		var converter = new JsonSerializerRuntimeTypeConverter(jsonSerializerOptions);

		var optionsObject = new RuntimeProjectManifestBuilderOptions(converter, builder);
		options.Invoke(optionsObject);
		return builder;
	}
}
