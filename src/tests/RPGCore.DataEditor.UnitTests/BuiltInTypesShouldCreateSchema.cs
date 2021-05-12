using NUnit.Framework;
using RPGCore.DataEditor.Manifest;
using RPGCore.DataEditor.Manifest.Source.RuntimeSource;
using RPGCore.DataEditor.Manifest.Source.JsonSerializer;
using RPGCore.DataEditor.UnitTests.Utility;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace RPGCore.DataEditor.UnitTests
{
	[TestFixture(TestOf = typeof(IRuntimeTypeConverter))]
	public class BuiltInTypesShouldCreateSchema
	{
		[EditorType]
		private class TestChildObject
		{
			public string ChildFieldA { get; set; } = "Child Value A";
			public string ChildFieldB { get; set; } = "Child Value B";
		}

		[EditorType]
		private class TestRootObject
		{
			public long[] TestArrayA { get; set; } = Array.Empty<long>();
			public long[] TestArrayB { get; set; } = new long[] { 1, 2, 3, 4 };
			public long[]? TestArrayC { get; set; } = null;
			public long[]? TestArrayD { get; set; } = Array.Empty<long>();
			public long[]? TestArrayE { get; set; } = new long[] { 1, 2, 3, 4 };

			public TestChildObject TestChildObjectA { get; set; } = new TestChildObject();
			public TestChildObject? TestChildObjectB { get; set; } = null;
			public TestChildObject? TestChildObjectC { get; set; } = new TestChildObject();

			public Dictionary<string, TestChildObject>? TestChildObjectDictionary { get; set; } = new Dictionary<string, TestChildObject>()
			{
				["First"] = new TestChildObject(),
				["Second"] = new TestChildObject()
			};
			public TestChildObject[] TestChildObjectArray { get; set; } = new TestChildObject[1]{
				new TestChildObject()
			};

			public Dictionary<string, string> TestDictionaryA { get; set; } = new Dictionary<string, string>();
			public Dictionary<string, string> TestDictionaryB { get; set; } = new Dictionary<string, string>()
			{
				["First"] = "First Value",
				["Second"] = "Second Value"
			};
			public Dictionary<string, string>? TestDictionaryC { get; set; } = null;
			public Dictionary<string, string>? TestDictionaryD { get; set; } = new Dictionary<string, string>();
			public Dictionary<string, string>? TestDictionaryE { get; set; } = new Dictionary<string, string>()
			{
				["First"] = "First Value",
				["Second"] = "Second Value"
			};

			public int TestIntegerA { get; set; } = 0;
			public int TestIntegerB { get; set; } = 100;
			public int? TestIntegerC { get; set; } = null;
			public int? TestIntegerD { get; set; } = 400;
			public long TestLongA { get; set; } = 0;
			public long TestLongB { get; set; } = 100;
			public long? TestLongC { get; set; } = null;
			public long? TestLongD { get; set; } = 400;
			public string TestStringA { get; set; } = string.Empty;
			public string TestStringB { get; set; } = "Test Value 1";
			public string? TestStringC { get; set; } = null;
			public string? TestStringD { get; set; } = string.Empty;
			public string? TestStringE { get; set; } = "Test Value 2";
		}

		[Test, Parallelizable]
		public void Serialize()
		{
			var serializer = new JsonSerializerOptions();
			var schema = ProjectManifestBuilder.Create()
				.UseTypesFromJsonSerializer(serializer, options =>
				{
					options.UseFrameworkTypes();
					options.UseType(typeof(TestRootObject));
					options.UseType(typeof(TestChildObject));
				})
				.Build();

			var session = new EditorSession(schema, new JsonEditorSerializer());

			var file = session.EditFile()
				.WithType(new TypeName("TestRootObject"))
				.SaveTo(new ConsoleLogSaver())
				.Build();

			file.Save();
		}
	}
}
