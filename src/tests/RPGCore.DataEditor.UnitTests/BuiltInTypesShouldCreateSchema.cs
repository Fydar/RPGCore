using NUnit.Framework;
using RPGCore.Data;
using RPGCore.DataEditor.Manifest;
using RPGCore.DataEditor.Manifest.Source.JsonSerializer;
using RPGCore.DataEditor.Manifest.Source.RuntimeSource;
using RPGCore.DataEditor.UnitTests.Utility;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace RPGCore.DataEditor.UnitTests
{
	[TestFixture(TestOf = typeof(IRuntimeTypeConverter))]
	public class BuiltInTypesShouldCreateSchema
	{
		[EditableType]
		private class TestChildObject
		{
			public string ChildFieldA { get; set; } = "Child Value A";
			public string ChildFieldB { get; set; } = "Child Value B";
		}

		[EditableType]
		private class TestRootObject
		{
			public int IntZero { get; set; } = 0;
			public int IntValue { get; set; } = 100;
			public int? IntNullableNull { get; set; } = null;
			public int? IntNullableValue { get; set; } = 400;

			public long LongZero { get; set; } = 0;
			public long LongValue { get; set; } = 100;
			public long? LongNullableNull { get; set; } = null;
			public long? LongNullableValue { get; set; } = 400;

			public string StringEmpty { get; set; } = string.Empty;
			public string StringValue { get; set; } = "Test Value 1";
			public string? StringNullableNull { get; set; } = null;
			public string? StringNullableEmpty { get; set; } = string.Empty;
			public string? StringNullableValue { get; set; } = "Test Value 2";

			public long[] LongArrayEmpty { get; set; } = Array.Empty<long>();
			public long[] LongArrayValue { get; set; } = new long[] { 1, 2, 3, 4 };
			public long[]? LongArrayNullableNull { get; set; } = null;
			public long[]? LongArrayNullableEmpty { get; set; } = Array.Empty<long>();
			public long[]? LongArrayNullableValue { get; set; } = new long[] { 1, 2, 3, 4 };

			public TestChildObject ObjectInstated { get; set; } = new TestChildObject();
			public TestChildObject? ObjectNullableNull { get; set; } = null;
			public TestChildObject? ObjectNullableInstated { get; set; } = new TestChildObject();

			public TestChildObject[] ArrayObjectEmpty { get; set; } = Array.Empty<TestChildObject>();
			public TestChildObject[] ArrayObjectValue { get; set; } = new TestChildObject[1]{
				new TestChildObject()
			};
			public TestChildObject[]? ArrayObjectNullableNull { get; set; } = null;
			public TestChildObject[]? ArrayObjectNullableEmpty { get; set; } = Array.Empty<TestChildObject>();
			public TestChildObject[]? ArrayObjectNullableValue { get; set; } = new TestChildObject[1]{
				new TestChildObject()
			};

			public Dictionary<string, string> DictionaryStringStringEmpty { get; set; } = new Dictionary<string, string>();
			public Dictionary<string, string> DictionaryStringStringValue { get; set; } = new Dictionary<string, string>()
			{
				["First"] = "First Value",
				["Second"] = "Second Value"
			};
			public Dictionary<string, string>? DictionaryNullableStringStringNull { get; set; } = null;
			public Dictionary<string, string>? DictionaryNullableStringStringEmpty { get; set; } = new Dictionary<string, string>();
			public Dictionary<string, string>? DictionaryNullableStringStringValue { get; set; } = new Dictionary<string, string>()
			{
				["First"] = "First Value",
				["Second"] = "Second Value"
			};

			public Dictionary<string, TestChildObject> DictionaryStringObjectEmpty { get; set; } = new Dictionary<string, TestChildObject>();
			public Dictionary<string, TestChildObject> DictionaryStringObjectValue { get; set; } = new Dictionary<string, TestChildObject>()
			{
				["First"] = new TestChildObject(),
				["Second"] = new TestChildObject()
			};
			public Dictionary<string, TestChildObject>? DictionaryNullableStringObjectNull { get; set; } = null;
			public Dictionary<string, TestChildObject>? DictionaryNullableStringObjectEmpty { get; set; } = new Dictionary<string, TestChildObject>();
			public Dictionary<string, TestChildObject>? DictionaryNullableStringObjectValue { get; set; } = new Dictionary<string, TestChildObject>()
			{
				["First"] = new TestChildObject(),
				["Second"] = new TestChildObject()
			};
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
				.SaveTo(new MockFileSink())
				.Build();

			file.Save();
		}
	}
}
