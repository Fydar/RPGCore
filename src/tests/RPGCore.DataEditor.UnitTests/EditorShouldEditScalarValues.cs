using NUnit.Framework;
using RPGCore.DataEditor.Files;
using RPGCore.DataEditor.Manifest;
using RPGCore.DataEditor.Manifest.Source.JsonSerializer;
using RPGCore.DataEditor.Manifest.Source.RuntimeSource;
using RPGCore.DataEditor.UnitTests.Utility;
using System.Text.Json;

namespace RPGCore.DataEditor.UnitTests
{
	[TestFixture(TestOf = typeof(EditorSession))]
	public class EditorShouldEditScalarValues
	{
		[EditableType]
		private class KitchenSinkContainer
		{
			public string StringValue { get; set; } = "Placeholder";
			public int IntValue { get; set; } = 1;
			public long LongValue { get; set; } = 100;
			public bool BoolValue { get; set; } = false;
		}

		private readonly ProjectManifest schema;
		private readonly EditorSession session;

		public EditorShouldEditScalarValues()
		{
			var serializer = new JsonSerializerOptions();

			schema = ProjectManifestBuilder.Create()
				.UseTypesFromJsonSerializer(serializer, options =>
				{
					options.UseFrameworkTypes();
					options.UseType(typeof(KitchenSinkContainer));
				})
				.Build();

			session = new EditorSession(schema, new JsonEditorSerializer());
		}

		[Test, Parallelizable]
		public void LoadCommentsFromJson()
		{
			var file = session.EditFile()
				.WithType(new TypeName("KitchenSinkContainer"))
				.LoadFrom(new TypeDefaultLoader())
				.SaveTo(new MockFileSink())
				.Build();

			file.Save();

			AssertUtility.AssertThatTypeIsEqualTo(file.Root, out EditorObject rootObject);

			AssertUtility.AssertThatTypeIsEqualTo(rootObject.Fields["StringValue"]?.Value, out EditorScalarValue stringValue);
			stringValue.Value = "Set to a value...";

			AssertUtility.AssertThatTypeIsEqualTo(rootObject.Fields["IntValue"]?.Value, out EditorScalarValue intValue);
			intValue.Value = 64;

			AssertUtility.AssertThatTypeIsEqualTo(rootObject.Fields["LongValue"]?.Value, out EditorScalarValue longValue);
			longValue.Value = 512;

			AssertUtility.AssertThatTypeIsEqualTo(rootObject.Fields["BoolValue"]?.Value, out EditorScalarValue boolValue);
			boolValue.Value = true;

			file.Save();
		}
	}
}
