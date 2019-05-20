using System;
using System.IO;
using System.Reflection;

namespace RPGCore.Packages.Tool
{
    class Program
    {
        enum ExitCode : int
        {
            Success = 0,
            InvalidLogin = 1,
            InvalidFilename = 2,
            UnknownError = 10
        }

        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                var versionString = Assembly.GetEntryAssembly()
                                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                                .InformationalVersion
                                .ToString();

                Console.WriteLine($"bpack v{versionString}");
                Console.WriteLine("Usage:");
                Console.WriteLine("  bpack build");
                Console.WriteLine("    Builds the .bproj at the active directory.");
                return (int)ExitCode.Success;
            }

            if (args[0].Equals("build", StringComparison.InvariantCultureIgnoreCase))
            {
                var folder = new DirectoryInfo("./");
                var bprojs = folder.GetFiles("*.bproj", SearchOption.TopDirectoryOnly);

                if (bprojs.Length != 1)
                {
                    if (bprojs.Length == 0)
                    {
                        Console.WriteLine("No \".bproj\" files found.");
                        return (int)ExitCode.InvalidFilename;
                    }
                    else
                    {
                        Console.WriteLine("Multiple \".bproj\" files found.");
                        return (int)ExitCode.InvalidFilename;
                    }
                }
                else
                {
                    var bproj = bprojs[0];

                    var project = ProjectExplorer.Load(bproj.DirectoryName);

                    string exportDirectory = "./bin/";
                    Directory.CreateDirectory(exportDirectory);

                    project.Export(Path.Combine(exportDirectory, project.Name + ".bpkg"));
                }
            }
            else
            {
                var folder = new DirectoryInfo("./");
                var bprojs = folder.GetFiles("*.bproj", SearchOption.TopDirectoryOnly);

                if (bprojs.Length != 1)
                {
                    if (bprojs.Length == 0)
                    {
                        Console.WriteLine("No \".bproj\" files found.");
                        return (int)ExitCode.InvalidFilename;
                    }
                    else
                    {
                        Console.WriteLine("Multiple \".bproj\" files found.");
                        return (int)ExitCode.InvalidFilename;
                    }
                }
                else
                {
                    var bproj = bprojs[0];

                    var project = ProjectExplorer.Load(bproj.DirectoryName);

                    project.bProj.Save(bproj.FullName);
                }
            }
            
            return (int)ExitCode.Success;
        }
    }
}
