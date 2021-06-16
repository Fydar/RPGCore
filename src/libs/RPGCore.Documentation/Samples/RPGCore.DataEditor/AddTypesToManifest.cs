using RPGCore.Data;
using RPGCore.Data.Polymorphic.Inline;
using RPGCore.Data.Polymorphic.SystemTextJson;
using RPGCore.DataEditor.Manifest;
using RPGCore.DataEditor.Manifest.Source.JsonSerializer;
using System.IO;
using System.Text;
using System.Text.Json;

namespace RPGCore.Documentation.Samples.RPGCore.DataEditor
{
	public class AddTypesToManifest
	{
		[EditableType]
		private class SerializerBaseObject
		{
			public string ChildFieldA { get; set; } = "Child Value A";
		}

		[EditableType]
		private class SerializerChildObject : SerializerBaseObject
		{
			public string ChildFieldB { get; set; } = "Child Value B";
		}

		[PresentOutput(OutputFormat.Json, "manifest")]
		public static string Run()
		{
			#region addtype
			// Tell the data editor what serializer we are going to be using.
			var serializer = new JsonSerializerOptions()
				.UsePolymorphicSerialization(options =>
				{
					options.UseInline();
				});

			// "Schemas" tell the data editor about the data structures involved.
			var schema = ProjectManifestBuilder.Create()
				.UseTypesFromJsonSerializer(serializer, options =>
				{
					// Allow usage of .NET types
					options.UseFrameworkTypes();

					// Explicitly add types
					options.UseType(typeof(SerializerBaseObject));
					options.UseType(typeof(SerializerChildObject));
				})
				.Build();
			#endregion addtype

			using var stream = new MemoryStream();
			schema.WriteTo(stream);

			return Encoding.UTF8.GetString(stream.ToArray());
		}
	}
}
