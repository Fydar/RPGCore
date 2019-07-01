using System;

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

			simulator = new Simulator ();
			simulator.Start ();

			Console.ReadLine ();
		}
	}
}
