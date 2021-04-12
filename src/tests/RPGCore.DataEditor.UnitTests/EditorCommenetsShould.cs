using NUnit.Framework;
using RPGCore.DataEditor.CSharp;
using RPGCore.DataEditor.Manifest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RPGCore.DataEditor.UnitTests
{
	[TestFixture(TestOf = typeof(EditorSession))]
	public class EditorCommenetsShould
	{
		[EditorType]
		public class TestObjectWithComments
		{
			public string ChildFieldA { get; set; } = "Child Value A";
			public string ChildFieldB { get; set; } = "Child Value B";
		}

		[Test, Parallelizable]
		public void Serialize()
		{
			var schemaBuilder = ProjectManifestBuilder.Create()
				.UseFrameworkTypes()
				.UseAllTypesFromAppDomain(AppDomain.CurrentDomain, typeof(BuiltInTypesShould).Assembly);

			var schema = schemaBuilder.Build();

			var session = new EditorSession(schema, new JsonEditorSerializer());

			var file = session.EditFile(new SchemaQualifiedType("TestObjectWithComments"));

			string testData = "/* This is a file comment */{/* This is a comment, please keep. */ /* There's another comment here too. */ \"ChildFieldA\": /* this is a value comment */\"This is a value\"}";
			
			file.Root = session.Serializer.DeserializeValue(session, new SchemaQualifiedType("TestObjectWithComments"), Encoding.UTF8.GetBytes(testData));

			using var memoryStream = new MemoryStream();
			session.Serializer.SerializeValue(file.Root, memoryStream);

			memoryStream.Seek(0, SeekOrigin.Begin);
			using var reader = new StreamReader(memoryStream);

			string result = reader.ReadToEnd();
			Console.WriteLine(result);
		}
	}
}
