using RPGCore.Documentation.SyntaxHighlighting;
using RPGCore.Documentation.SyntaxHighlighting.Json;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace RPGCore.Documentation;

internal class Program
{
	private static void Main(string[] args)
	{
		ForceLoadDependencies();

		// Delete pre-existing samples
		SampleRenderer.GetDestinationDirectory().Delete(true);

		var directory = SampleRenderer.FindRepositoryDirectory();
		string basePath = Path.Combine(directory.FullName, "src/libs/RPGCore.Documentation/Samples");
		foreach (string file in Directory.GetFiles(basePath, "*", SearchOption.AllDirectories))
		{
			SampleRenderer.ExportSvgSample(basePath, file);
		}

		foreach (var type in typeof(Program).Assembly.GetTypes())
		{
			var methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (var method in methods)
			{
				foreach (var attribute in method.GetCustomAttributes<PresentOutputAttribute>())
				{
					if (method.GetParameters().Length == 0)
					{
						object? result = method.Invoke(null, null);

						if (attribute.Format == OutputFormat.Json)
						{
							if (result is string stringResult)
							{
								string folderName = type.FullName?.Replace("RPGCore.Documentation.Samples.", "") ?? "";

								folderName = folderName.Substring(0, folderName.LastIndexOf('.'));

								SampleRenderer.ExportSvgSample(JsonSyntax.ToCodeBlocks(stringResult), $"{folderName}/{type.Name}.{attribute.Name}");
							}
						}
					}
				}
			}
		}
	}

	private static void ForceLoadDependencies()
	{
		var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
		string[] alreadyLoadedPaths = loadedAssemblies.Select(a => a.Location).ToArray();
		string[] assemblyPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");

		foreach (string assemblyPath in assemblyPaths)
		{
			if (alreadyLoadedPaths.Contains(assemblyPath, StringComparer.InvariantCultureIgnoreCase))
			{
				continue;
			}
			try
			{
				var loaddedAssembly = AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(assemblyPath));
				loadedAssemblies.Add(loaddedAssembly);
			}
			catch
			{
			}
		}
	}
}
