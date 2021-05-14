using NUnit.Framework;
using RPGCore.DataEditor.Manifest;
using RPGCore.DataEditor.Manifest.Source.JsonSerializer;
using RPGCore.Data;
using RPGCore.DataEditor.UnitTests.Utility;
using System.Text.Json;

namespace RPGCore.DataEditor.UnitTests
{
	[TestFixture(TestOf = typeof(JsonEditorSerializer))]
	public class JsonEditorSerializerShould
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

		private readonly ProjectManifest schema;
		private readonly EditorSession session;

		public JsonEditorSerializerShould()
		{
			var serializer = new JsonSerializerOptions();

			schema = ProjectManifestBuilder.Create()
				.UseTypesFromJsonSerializer(serializer, options =>
				{
					options.UseFrameworkTypes();
					options.UseType(typeof(SerializerBaseObject));
					options.UseType(typeof(SerializerChildObject));
				})
				.Build();

			session = new EditorSession(schema, new JsonEditorSerializer());
		}

		[Test, Parallelizable]
		public void LoadCommentsFromJson()
		{
			string testData = "/* This is a file comment */{/* This is a comment, please keep. */ /* There's another comment here too. */ \"ChildFieldA\": /* this is a value comment */\"This is a value\"}";

			var file = session.EditFile()
				.WithType(new TypeName("SerializerChildObject"))
				.LoadFrom(new StringContentLoader(testData))
				.SaveTo(new MockFileSink())
				.Build();

			file.Save();

			AssertUtility.AssertThatTypeIsEqualTo(file.Root, out EditorObject rootObject);

			Assert.That(rootObject.Comments, Has.Count.EqualTo(1));
			Assert.That(rootObject.Comments[0], Is.EqualTo(" This is a file comment "));
		}

		[Test, Parallelizable]
		public void SaveWithAbstractBaseTypes()
		{
			var output = new MockFileSink();

			var file = session.EditFile()
				.WithType(new TypeName("SerializerBaseObject"))
				.LoadFrom(new StringContentLoader(@"
				{
					""$type"": ""SerializerChildObject"",
					""ChildFieldA"": ""Loaded 1"",
					""ChildFieldB"": ""Loaded 2""
				}"))
				.SaveTo(output)
				.Build();

			file.Save();
		}

		[Test, Parallelizable]
		public void SaveWithUnknownBaseTypes()
		{
			var output = new MockFileSink();

			var file = session.EditFile()
				.WithType(TypeName.Unknown)
				.LoadFrom(new StringContentLoader(@"
				{
					""$type"": ""SerializerChildObject"",
					""ChildFieldA"": ""Loaded 1"",
					""ChildFieldB"": ""Loaded 2""
				}"))
				.SaveTo(output)
				.Build();

			file.Save();
		}
	}
}
