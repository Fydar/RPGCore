using NUnit.Framework;
using RPGCore.DataEditor.CSharp;
using RPGCore.DataEditor.Manifest;
using System;
using System.Collections.Generic;
using System.IO;

namespace RPGCore.DataEditor.UnitTests
{
	[TestFixture(TestOf = typeof(BuiltInTypes))]
	public class BuiltInTypesShould
	{
		[Test, Parallelizable]
		public void DescribeDictionaryType()
		{
			var describedType = BuiltInTypes.DescribeType(typeof(Dictionary<string, int>));

			Assert.That(describedType.Identifier, Is.EqualTo("[Dictionary]"));
			Assert.That(describedType.TemplateTypes, Has.Length.EqualTo(2));
			Assert.That(describedType.TemplateTypes[0].Identifier, Is.EqualTo("string"));
			Assert.That(describedType.TemplateTypes[1].Identifier, Is.EqualTo("int"));
		}

		[EditorType]
		public class TestChildObject
		{
			public string ChildFieldA { get; set; } = "Child Value A";
			public string ChildFieldB { get; set; } = "Child Value B";
		}

		[EditorType]
		public class TestRootObject
		{
			public TestChildObject TestChildObjectA { get; set; } = new TestChildObject();
			public TestChildObject? TestChildObjectB { get; set; } = null;
			public TestChildObject? TestChildObjectC { get; set; } = new TestChildObject();
			public TestChildObject[] TestChildObjectArray { get; set; } = new TestChildObject[1]{
				new TestChildObject()
			};
			public Dictionary<string, TestChildObject>? TestChildObjectDictionary { get; set; } = new Dictionary<string, TestChildObject>()
			{
				["First"] = new TestChildObject(),
				["Second"] = new TestChildObject()
			};

			public string TestStringA { get; set; } = string.Empty;
			public string TestStringB { get; set; } = "Test Value 1";
			public string? TestStringC { get; set; } = null;
			public string? TestStringD { get; set; } = string.Empty;
			public string? TestStringE { get; set; } = "Test Value 2";

			public int TestIntegerA { get; set; } = 0;
			public int TestIntegerB { get; set; } = 100;
			public int? TestIntegerC { get; set; } = null;
			public int? TestIntegerD { get; set; } = 400;

			public long TestLongA { get; set; } = 0;
			public long TestLongB { get; set; } = 100;
			public long? TestLongC { get; set; } = null;
			public long? TestLongD { get; set; } = 400;

			public long[] TestArrayA { get; set; } = Array.Empty<long>();
			public long[] TestArrayB { get; set; } = new long[] { 1, 2, 3, 4 };
			public long[]? TestArrayC { get; set; } = null;
			public long[]? TestArrayD { get; set; } = Array.Empty<long>();
			public long[]? TestArrayE { get; set; } = new long[] { 1, 2, 3, 4 };

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
		}

		[Test, Parallelizable]
		public void BuildTypeSchema()
		{
			var schemaBuilder = ProjectManifestBuilder.Create()
				.UseFrameworkTypes();

			var schema = schemaBuilder.Build();


		}

		[Test, Parallelizable]
		public void Serialize()
		{
			var schemaBuilder = ProjectManifestBuilder.Create()
				.UseFrameworkTypes()
				.UseAllTypesFromAppDomain(AppDomain.CurrentDomain, typeof(BuiltInTypesShould).Assembly);

			var schema = schemaBuilder.Build();

			var session = new EditorSession(schema, new JsonEditorSerializer());

			var file = session.EditFile(new SchemaQualifiedType("TestRootObject"));

			using var memoryStream = new MemoryStream();
			session.Serializer.SerializeValue(file.Root, memoryStream);

			memoryStream.Seek(0, SeekOrigin.Begin);
			using var reader = new StreamReader(memoryStream);

			string result = reader.ReadToEnd();
			Console.WriteLine(result);
		}
	}
}
