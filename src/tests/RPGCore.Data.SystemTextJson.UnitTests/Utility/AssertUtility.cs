using NUnit.Framework;

namespace RPGCore.Data.UnitTests.Utility
{
	public static class AssertUtility
	{
		public static void AssertThatTypeIsEqualTo<A, B>(A source, out B output)
			where B : A
		{
			if (source is B casted)
			{
				output = casted;
			}
			else
			{
				throw new AssertionException($"Object was of the wrong type.");
			}
		}
	}
}
