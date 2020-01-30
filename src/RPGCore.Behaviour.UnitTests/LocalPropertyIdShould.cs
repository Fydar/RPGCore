using NUnit.Framework;

namespace RPGCore.Behaviour.UnitTests
{
	public class LocalPropertyIdShould
	{
		[Test]
		public void ParseString()
		{
			var parsedLocalPropertyId = new LocalPropertyId("0xff001000.Output");

			Assert.AreEqual(new LocalId(0xff001000), parsedLocalPropertyId.TargetIdentifier);
			Assert.AreEqual(1, parsedLocalPropertyId.PropertyPath.Length, "Property path should have one element");
			Assert.AreEqual("Output", parsedLocalPropertyId.PropertyPath[0], "Property path element should be \"Output\"");

			Assert.AreEqual(new LocalPropertyId(new LocalId(0xff001000), "Output"), parsedLocalPropertyId);
		}
	}
}
