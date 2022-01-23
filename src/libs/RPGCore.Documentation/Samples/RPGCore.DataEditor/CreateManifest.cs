using RPGCore.Data;
using RPGCore.Data.Polymorphic.Inline;
using RPGCore.Data.Polymorphic.SystemTextJson;
using RPGCore.DataEditor;
using RPGCore.DataEditor.Manifest;
using RPGCore.DataEditor.Manifest.Source.JsonSerializer;
using System.IO;
using System.Text.Json;

namespace RPGCore.Documentation.Samples.RPGCore.DataEditor;

public class CreateManifest
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

	public static void Run()
	{
		#region full
		#region create_from_dotnet
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

				// Can also automatically add types
				// options.UseAllTypesFromAppDomain(AppDomain.CurrentDomain);
				// options.UseAllTypesFromAssembly(typeof(SerializerBaseObject).Assembly);
			})
			.Build();
		#endregion create_from_dotnet

		#region save_and_load
		// You can save/load schemas from/to files
		// This allows your data editor to be completely seperate from your game code.
		var schemaFile = new FileInfo("schema.sch");
		schemaFile.Delete();
		using (var stream = schemaFile.OpenWrite())
		{
			schema.WriteTo(stream);
		}

		var loadedSchema = ProjectManifestBuilder.Load(File.ReadAllBytes(schemaFile.FullName))
			.Build();

		var session = new EditorSession(loadedSchema, new JsonEditorSerializer());
		#endregion save_and_load
		#endregion full
	}
}
