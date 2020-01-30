using NUnit.Framework;

namespace RPGCore.Behaviour.UnitTests
{
	public class LocalIdShould
	{
		[Test]
		public void CaseInvarientParsing()
		{
			int original = 1048471;

			string upperHex = original.ToString("X2");
			string lowerHex = original.ToString("x2");

			var upperId = new LocalId(upperHex);
			var lowerId = new LocalId(lowerHex);

			Assert.AreEqual(upperId, lowerId);
		}

		[Test]
		public void MatchesIntegerLiteral()
		{
			var stringLiteral = new LocalId("0xff001000");
			var integerLiteral = new LocalId(0xff001000);

			Assert.AreEqual(stringLiteral, integerLiteral);
		}

		[Test]
		public void PrefixInvarientParsing()
		{
			int original = 1048471;

			string unprefixed = original.ToString("x2");
			string prefixed = "0x" + unprefixed;

			var prefixedId = new LocalId(prefixed);
			var unprefixedId = new LocalId(unprefixed);

			Assert.AreEqual(prefixedId, unprefixedId);
		}
	}
}
