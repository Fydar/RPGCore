using NUnit.Framework;
using System.IO;

namespace RPGCore.Packages.UnitTests.Utilities
{
	internal static class TestUtilities
	{
		public static string CreateFilePath(string name)
		{
			return Path.Combine(
				TestContext.CurrentContext.Test.ClassName,
				TestContext.CurrentContext.Test.Name, name);
		}
	}
}
