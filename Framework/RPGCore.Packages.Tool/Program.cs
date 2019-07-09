using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace RPGCore.Packages.Tool
{
	internal class Program
	{
		private enum ExitCode : int
		{
			Success = 0,
			InvalidLogin = 1,
			InvalidFilename = 2,
			UnknownError = 10
		}

		private static int Main (string[] args)
		{
			if (args.Length == 0)
			{
				string versionString = Assembly.GetEntryAssembly ()
								.GetCustomAttribute<AssemblyInformationalVersionAttribute> ()
								.InformationalVersion
								.ToString ();

				Console.WriteLine ($"bpack v{versionString}");
				Console.WriteLine ("Usage:");
				Console.WriteLine ("  bpack build [path]");
				Console.WriteLine ("    Builds the .bproj at the active directory.");
				return (int)ExitCode.Success;
			}

			string command = args[0];
			string subCommand = args.Length > 1 ? args[1] : "";

			if (command.Equals ("build", StringComparison.InvariantCultureIgnoreCase))
			{
				FileInfo file;
				if (subCommand == "")
				{
					file = FindFileOfType (".bproj");
				}
				else
				{
					file = new FileInfo (subCommand);
				}

				var project = ProjectExplorer.Load (file.DirectoryName, new List<ResourceImporter> ());

				string exportDirectory = "./bin/";
				Directory.CreateDirectory (exportDirectory);

				project.Export (exportDirectory);
			}
			else if (command.Equals ("format", StringComparison.InvariantCultureIgnoreCase))
			{
				if (subCommand == "all")
				{
					var files = FindFilesOfType (".csproj", SearchOption.AllDirectories);

					foreach (var file in files)
					{
						var projectFile = XmlProjectFile.Load (file.FullName);

						projectFile.Format ();

						projectFile.Save (file.FullName);
					}
				}
				else
				{
					var file = FindFileOfType (".bproj");
					if (file != null)
					{
						var project = ProjectExplorer.Load (file.DirectoryName, new List<ResourceImporter> ());

						project.Definition.Format ();

						project.Definition.Save (file.FullName);
					}
					else
					{
						file = FindFileOfType (".csproj");

						var projectFile = XmlProjectFile.Load (file.FullName);

						projectFile.Format ();

						projectFile.Save (file.FullName);
					}
				}
			}

			return (int)ExitCode.Success;
		}

		private static FileInfo FindFileOfType (string extension)
		{
			var folder = new DirectoryInfo ("./");
			var bprojs = folder.GetFiles ($"*{extension}", SearchOption.TopDirectoryOnly);
			if (bprojs.Length != 1)
			{
				if (bprojs.Length == 0)
				{
					Console.WriteLine ($"No \"{extension}\" files found.");
					return null;
				}
				else
				{
					Console.WriteLine ($"Multiple \"{extension}\" files found.");
					return null;
				}
			}

			return bprojs[0];
		}

		private static FileInfo[] FindFilesOfType (string extension, SearchOption searchOption = SearchOption.TopDirectoryOnly)
		{
			var folder = new DirectoryInfo ("./");
			return folder.GetFiles ($"*{extension}", searchOption);
		}
	}
}
