using NUnit.Framework;
using RPGCore.Data.Polymorphic.Configuration;
using System.IO;

namespace RPGCore.Data.UnitTests.Utility
{
	public static class RenderingUtility
	{
		public static void RenderConfiguration(TextWriter output, PolymorphicConfiguration polymorphicConfiguration)
		{
			output.WriteLine($"Descriminator: {polymorphicConfiguration.DescriminatorName}");
			output.WriteLine($"Case Insensitive: {polymorphicConfiguration.CaseInsensitive}");
			output.WriteLine();

			foreach (var baseType in polymorphicConfiguration.BaseTypes)
			{
				output.WriteLine(baseType.BaseType.Name);

				foreach (var subType in baseType.SubTypes)
				{
					output.WriteLine($"- {subType.Name} ({string.Join(", ", subType.Aliases)})");
				}
			}
		}
	}
}
