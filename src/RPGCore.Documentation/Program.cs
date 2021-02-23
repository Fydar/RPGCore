using System;
using System.IO;

namespace RPGCore.Documentation
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");

			var directory = FindSourceDirectory();

			Console.WriteLine(directory.FullName);
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
