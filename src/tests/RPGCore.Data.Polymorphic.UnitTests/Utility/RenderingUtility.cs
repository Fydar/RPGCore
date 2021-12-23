using RPGCore.Data.Polymorphic;
using System.IO;

namespace RPGCore.Data.UnitTests.Utility
{
	public static class RenderingUtility
	{
		public static void RenderConfiguration(TextWriter output, PolymorphicOptions options)
		{
			output.WriteLine($"Descriminator: {options.DescriminatorName}");
			output.WriteLine($"Case Insensitive: {options.CaseInsensitive}");
			output.WriteLine();

			foreach (var baseType in options.BaseTypes)
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
