using RPGCore.CLI.Commands;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace RPGCore.CLI
{
	internal sealed class Program
	{
		private static Task<int> Main(string[] args)
		{
			var rootCommand = new RootCommand ("Command-line tool for RPGCore")
			{
				new BuildCommand("build", "Builds the current .bproj."),
				new FormatCommand("format", "Formats a .bproj file.")
			};
			rootCommand.Name = "bpack";

			return rootCommand.InvokeAsync (args);
		}
	}
}
