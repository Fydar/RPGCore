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

			var directory = new DirectoryInfo ("Content");
			if (!directory.Exists)
			{
				directory = new DirectoryInfo ("RPGCoreUnity/Content");
			}
			Directory.SetCurrentDirectory (directory.Parent.FullName);

			simulator = new Simulator ();
			simulator.Start ();

			Console.ReadLine ();
		}
	}
}
