using RPGCore.Data;
using RPGCore.DataEditor;
using RPGCore.DataEditor.Files;
using RPGCore.DataEditor.Manifest;
using RPGCore.DataEditor.Manifest.Source.JsonSerializer;
using System.IO;
using System.Text.Json;

namespace RPGCore.Documentation.Samples.EntityComponentSystemSample
{
	public class LoadFromFile
	{
		#region data_type
		[EditableType]
		public class Weapon
		{
			public string Name { get; set; } = "Longsword";
			public int Damage { get; set; } = 20;
			public long Durability { get; set; } = 100;
			public bool IsEnchantable { get; set; } = true;
		}
		#endregion data_type

		public static void Run()
		{
			#region full
			var serializer = new JsonSerializerOptions();

			var schema = ProjectManifestBuilder.Create()
				.UseTypesFromJsonSerializer(serializer, options =>
				{
					options.UseFrameworkTypes();
					options.UseType(typeof(Weapon));
				})
				.Build();

			#region editing
			#region configure
			var session = new EditorSession(schema, new JsonEditorSerializer());

			var file = session.EditFile()
				.WithType(new TypeName("Weapon"))
				.LoadFrom(new TypeDefaultLoader())
				.SaveTo(new FileSystemFile(new FileInfo("data.json")))
				.Build();
			#endregion configure

			var rootObject = (EditorObject)file.Root;

			var nameValue = (EditorScalarValue)rootObject.Fields["Name"]!.Value;
			var damageValue = (EditorScalarValue)rootObject.Fields["Damage"]!.Value;

			nameValue.Value = "Excalibur";
			damageValue.Value = 50;

			file.Save();
			#endregion editing
			#endregion full
		}
	}
}
