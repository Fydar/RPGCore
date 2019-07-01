using System;
using System.IO;

namespace RPGCore.Runner
{
	internal class Program
	{
		public static Simulator simulator;

		private static void Main (string[] args)
		{
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine ("-----------------------");
			Console.Write (" RPGCore ");
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write ("2.0");
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.Write (" Framework\n");
			Console.WriteLine ("-----------------------");

			if (!File.Exists ("Content"))
			{
				var directory = new DirectoryInfo ("bin/Debug/netcoreapp2.2");
				Directory.SetCurrentDirectory (directory.FullName);
			}

			simulator = new Simulator ();
			simulator.Start ();

			Console.ReadLine ();
		}
	}
}
