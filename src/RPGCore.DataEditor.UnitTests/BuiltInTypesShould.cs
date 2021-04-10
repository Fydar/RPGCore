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
		public class EditorObjectExample
		{
			public string Name { get; set; } = string.Empty;
			public string? Description { get; set; }
			public string? Tagline { get; set; } = string.Empty;

			public int RequiredLevel { get; set; } = 10;
			public long DateCreated { get; set; }
			public long? DateDeleted { get; set; }
			public long[] Timestamps { get; set; } = Array.Empty<long>();
			public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
			public Dictionary<string, string>? Tags { get; set; }
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

			var file = session.EditFile(new SchemaQualifiedType("EditorObjectExample"));

			using var memoryStream = new MemoryStream();
			session.Serializer.SerializeValue(file.Root, memoryStream);

			memoryStream.Seek(0, SeekOrigin.Begin);
			using var reader = new StreamReader(memoryStream);

			string result = reader.ReadToEnd();
			Console.WriteLine(result);
		}
	}
}
