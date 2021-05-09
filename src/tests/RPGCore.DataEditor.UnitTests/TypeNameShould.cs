using NUnit.Framework;
using RPGCore.DataEditor.Manifest;
using System;

namespace RPGCore.DataEditor.UnitTests
{
	[TestFixture(TestOf = typeof(TypeName))]
	public class TypeNameShould
	{
		[Test, Parallelizable]
		[TestCase("Base")]
		[TestCase("Base?")]
		[TestCase("Base?<Temp1, Temp2, Temp3>")]
		[TestCase("Base?<Temp1, Temp2>")]
		[TestCase("Base?<Temp1>")]
		[TestCase("Base<Temp1, Temp2, Temp3>")]
		[TestCase("Base<Temp1, Temp2>")]
		[TestCase("Base<Temp1?, Temp2?>")]
		[TestCase("Base<Temp1?>")]
		[TestCase("Base<Temp1>")]
		public void FromStringShould(string beforeString)
		{
			var afterType = TypeName.FromString(beforeString);
			string afterString = afterType.ToString();

			Console.WriteLine(beforeString);
			Console.WriteLine(afterString);

			Assert.That(afterString, Is.EqualTo(beforeString));
		}
	}
}
