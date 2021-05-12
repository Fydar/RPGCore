using NUnit.Framework;
using RPGCore.DataEditor.Manifest;
using RPGCore.DataEditor.Manifest.Source.RuntimeSource;
using RPGCore.DataEditor.Manifest.Source.JsonSerializer;
using System.Collections.Generic;
using System.Text.Json;

namespace RPGCore.DataEditor.UnitTests
{
	[TestFixture(TestOf = typeof(RuntimeUtility))]
	public class RuntimeUtilityShould
	{
		[Test, Parallelizable]
		public void DescribeDictionaryType()
		{
			var serializer = new JsonSerializerOptions();
			var schema = ProjectManifestBuilder.Create()
				.UseTypesFromJsonSerializer(serializer, options =>
				{
					options.UseFrameworkTypes();
				})
				.Build();

			var session = new EditorSession(schema, new JsonEditorSerializer());

			var describedType = RuntimeUtility.DescribeType(typeof(Dictionary<string, int>));

			Assert.That(describedType.Identifier, Is.EqualTo("[Dictionary]"));
			Assert.That(describedType.TemplateTypes, Has.Length.EqualTo(2));
			Assert.That(describedType.TemplateTypes[0].Identifier, Is.EqualTo("string"));
			Assert.That(describedType.TemplateTypes[1].Identifier, Is.EqualTo("int"));
		}
	}
}
