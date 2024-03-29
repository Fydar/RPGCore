﻿using RPGCore.Data;
using RPGCore.DataEditor;
using RPGCore.DataEditor.Files;
using RPGCore.DataEditor.Manifest;
using RPGCore.DataEditor.Manifest.Source.JsonSerializer;
using System.IO;
using System.Text.Json;

namespace RPGCore.Documentation.Samples.RPGCore.DataEditor;

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

	[PresentOutput(OutputFormat.Json, "before")]
	public static string DefaultWeapon()
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

		file.Save();
		#endregion editing
		#endregion full

		return File.ReadAllText(new FileInfo("data.json").FullName);
	}

	[PresentOutput(OutputFormat.Json, "output")]
	public static string Run()
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

		#region configure
		var session = new EditorSession(schema, new JsonEditorSerializer());

		var file = session.EditFile()
			.WithType(new TypeName("Weapon"))
			.LoadFrom(new TypeDefaultLoader())
			.SaveTo(new FileSystemFile(new FileInfo("data.json")))
			.Build();
		#endregion configure

		#region editing
		var rootObject = (EditorObject)file.Root;

		rootObject.Comments.Add(" important comments ");

		var nameValue = (EditorScalarValue)rootObject.Fields["Name"]!.Value.Value;
		var damageValue = (EditorScalarValue)rootObject.Fields["Damage"]!.Value.Value;
		var durabilityValue = (EditorScalarValue)rootObject.Fields["Durability"]!.Value.Value;
		var isEnchantableValue = (EditorScalarValue)rootObject.Fields["IsEnchantable"]!.Value.Value;

		nameValue.Value = "Excalibur";
		damageValue.Value = 50;
		durabilityValue.Value = 800;
		isEnchantableValue.Value = true;

		file.Save();
		#endregion editing
		#endregion full

		return File.ReadAllText(new FileInfo("data.json").FullName);
	}
}
