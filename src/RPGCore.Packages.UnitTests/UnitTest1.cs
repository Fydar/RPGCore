using NUnit.Framework;

namespace Tests
{
	public class Tests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test, Parallelizable]
		public void Test1()
		{
			Assert.Pass();
		}
	}
}
