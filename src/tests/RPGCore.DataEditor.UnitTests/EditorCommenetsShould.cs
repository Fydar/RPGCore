using NUnit.Framework;
using RPGCore.Data;
using RPGCore.DataEditor.Manifest;
using RPGCore.DataEditor.Manifest.Source.JsonSerializer;
using RPGCore.DataEditor.UnitTests.Utility;
using System;
using System.Text.Json;

namespace RPGCore.DataEditor.UnitTests;

[TestFixture(TestOf = typeof(EditorSession))]
public class EditorCommenetsShould
{
	[EditableType]
	private class TestObjectWithComments
	{
		public string ChildFieldA { get; set; } = "Child Value A";
		public string ChildFieldB { get; set; } = "Child Value B";
	}

	private readonly ProjectManifest schema;
	private readonly EditorSession session;

	public EditorCommenetsShould()
	{
		var serializer = new JsonSerializerOptions();

		schema = ProjectManifestBuilder.Create()
			.UseTypesFromJsonSerializer(serializer, options =>
			{
				options.UseFrameworkTypes();
				options.UseType(typeof(TestObjectWithComments));
			})
			.Build();

		session = new EditorSession(schema, new JsonEditorSerializer());
	}

	[Test, Parallelizable]
	public void LoadCommentsFromJson()
	{
		string testData =
			"/* This is a file comment */\n" +
			"{\n" +
			"    /* This is a comment, please keep. */\n" +
			"    /* There's another comment here too. */\n" +
			"    \"ChildFieldA\": /* this is a value comment */ \"This is a value\",\n" +
			"    /* Another field comment here too. */\n" +
			"    \"ChildFieldB\": /* this is a value comment */ \"This is a value\"\n" +
			"}";

		Console.WriteLine("Original");
		Console.WriteLine();
		Console.WriteLine(testData);
		Console.WriteLine();

		var saver = new MockFileSink();

		var file = session.EditFile()
			.WithType(new TypeName("TestObjectWithComments"))
			.LoadFrom(new StringContentLoader(testData))
			.SaveTo(saver)
			.Build();

		file.Save();

		Console.WriteLine();

		file = session.EditFile()
			.WithType(new TypeName("TestObjectWithComments"))
			.LoadFrom(new StringContentLoader(saver.SaveHistory[0]))
			.SaveTo(saver)
			.Build();

		file.Save();

		Console.WriteLine();

		AssertUtility.AssertThatTypeIsEqualTo(file.Root, out EditorObject rootObject);

		Assert.That(rootObject.Comments, Has.Count.EqualTo(1));
		Assert.That(rootObject.Comments[0], Is.EqualTo(" This is a file comment "));
	}
}
