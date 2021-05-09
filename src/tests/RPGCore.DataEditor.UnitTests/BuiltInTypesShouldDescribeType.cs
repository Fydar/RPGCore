using NUnit.Framework;
using RPGCore.DataEditor.CSharp;
using System.Collections.Generic;

namespace RPGCore.DataEditor.UnitTests
{
	[TestFixture(TestOf = typeof(BuiltInTypes))]
	public class BuiltInTypesShouldDescribeType
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
	}
}
