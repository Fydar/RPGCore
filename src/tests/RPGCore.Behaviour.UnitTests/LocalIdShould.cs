using NUnit.Framework;

namespace RPGCore.Behaviour.UnitTests;

[TestFixture(TestOf = typeof(LocalId))]
public class LocalIdShould
{
	[Test, Parallelizable]
	public void CaseInvarientParsing()
	{
		int original = 1048471;

		string upperHex = original.ToString("X2");
		string lowerHex = original.ToString("x2");

		_ = LocalId.TryParse(upperHex, out var upperId);
		_ = LocalId.TryParse(lowerHex, out var lowerId);

		Assert.AreEqual(upperId, lowerId);
	}

	[Test, Parallelizable]
	public void MatchesIntegerLiteral()
	{
		_ = LocalId.TryParse("0xff001000", out var stringLiteral);
		var integerLiteral = new LocalId(0xff001000);

		Assert.AreEqual(stringLiteral, integerLiteral);
	}

	[Test, Parallelizable]
	public void PrefixInvarientParsing()
	{
		int original = 1048471;

		string unprefixed = original.ToString("x2");
		string prefixed = "0x" + unprefixed;

		_ = LocalId.TryParse(prefixed, out var prefixedId);
		_ = LocalId.TryParse(unprefixed, out var unprefixedId);

		Assert.AreEqual(prefixedId, unprefixedId);
	}
}
