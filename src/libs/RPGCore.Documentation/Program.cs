using RPGCore.Documentation.Internal;
using RPGCore.Documentation.Samples.AddNodeSample;
using RPGCore.Documentation.Samples.EntityComponentSystemSample;
using System;
using System.IO;

namespace RPGCore.Documentation
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var loadType = typeof(AddNode);
			loadType = typeof(EntityComponentSystemSample);
			EntityComponentSystemSample.Run();

			ExportSamples("AddNodeSample");
			ExportSamples("EntityComponentSystemSample");
		}
		private static void ExportSamples(string filename)
		{
			var sampleFile = GetSampleFile($"{filename}.cs");

			string sampleContent = File.ReadAllText(sampleFile.FullName);

			foreach (var builder in SampleParser.HtmlHighlight(sampleContent))
			{
				Console.WriteLine();
				Console.WriteLine($"-[{builder.Name}]-");
				Console.WriteLine();

				FileInfo destination;
				if (builder.Name == "")
				{
					destination = GetDestinationFile($"{filename}.html");
				}
				else
				{
					destination = GetDestinationFile($"{filename}.{builder.Name}.html");
				}
				destination.Directory.Create();

				destination.Delete();
				using var fs = destination.OpenWrite();
				using var sw = new StreamWriter(fs);

				sw.WriteLine($@"<!DOCTYPE html>
<html lang=""en"">
<head>
	<meta charset=""UTF-8"">
	<meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
	<title>{filename} {builder.Name }</title>
	<link rel=""stylesheet"" asp-append-version=""true"" href=""../patio.min.css"" />
</head>
<body>
	<div class=""container"">
		<table class=""code-table"">");

				for (int i = 0; i < builder.Lines.Length; i++)
				{
					string line = builder.Lines[i];
					sw.Write("\t\t\t<tr>\n\t\t\t\t<th>");
					sw.Write(i + 1);
					sw.Write("</th>\n\t\t\t\t<td>");
					sw.Write(line);
					sw.Write("</td>\n\t\t\t</tr>\n");
				}
				sw.WriteLine(@"
		</table>
	</div>
</body>
</html>");
			}
		}

		public static FileInfo GetSampleFile(string file)
		{
			var directory = FindSourceDirectory();
			string sampleFile = Path.Combine(directory.FullName, "src/libs/RPGCore.Documentation/Samples", file);
			return new FileInfo(sampleFile);
		}

		public static FileInfo GetDestinationFile(string file)
		{
			var directory = FindSourceDirectory();
			string sampleFile = Path.Combine(directory.FullName, "docs/samples", file);
			return new FileInfo(sampleFile);
		}

		private static DirectoryInfo FindSourceDirectory()
		{
			var directory = new DirectoryInfo(Environment.CurrentDirectory);
			while (directory != null)
			{
				if (directory.Name.Equals("RPGCore", StringComparison.OrdinalIgnoreCase))
				{
					return directory;
				}

				directory = directory.Parent;
			}
			return directory;
		}
	}
}
