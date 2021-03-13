using System;
using System.IO;

namespace RPGCore.Documentation
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");

			Console.WriteLine("----------");

			string sample = GetSample("AddNodeSample.cs", "default");
			Console.WriteLine(sample);
		}

		public static string GetSample(string file, string section)
		{
			var directory = FindSourceDirectory();

			Console.WriteLine(directory.FullName);

			string sampleFile = Path.Combine(directory.FullName, "src/RPGCore.Documentation/Samples", file);
			var sampleFileInfo = new FileInfo(sampleFile);

			return "";
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
