using RPGCore.Packages;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;

namespace RPGCore.CLI.Commands
{
	public class FormatCommand : Command
	{
		public FormatCommand(string name, string description = null) : base(name, description)
		{
			AddArgument(new Argument<FileInfo>("project", null)
			{
				Description = "A project file to use."
			});

			Handler = CommandHandler.Create((ParseResult parseResult, IConsole console) =>
		   {
			   var project = parseResult.ValueForOption<FileInfo>("project");

			   if (project == null)
			   {
				   project = FindFileOfType(".bproj");
			   }
			   var importPipeline = new ImportPipeline();

			   var projectExplorer = ProjectExplorer.Load(project.DirectoryName, importPipeline);

			   projectExplorer.Definition.SaveChanges();
		   });
		}

		private static FileInfo FindFileOfType(string extension)
		{
			var folder = new DirectoryInfo("./");
			var bprojs = folder.GetFiles($"*{extension}", SearchOption.TopDirectoryOnly);
			if (bprojs.Length != 1)
			{
				if (bprojs.Length == 0)
				{
					Console.WriteLine($"No \"{extension}\" files found.");
					return null;
				}
				else
				{
					Console.WriteLine($"Multiple \"{extension}\" files found.");
					return null;
				}
			}

			return bprojs[0];
		}
	}
}
