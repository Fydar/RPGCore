using System;

namespace Runner
{
	class Program
	{
		public static Simulator simulator;

		static void Main(string[] args)
		{
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine ("-----------------------");
			Console.Write (" RPGCore ");
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write ("2.0");
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.Write (" Framework\n");
			Console.WriteLine ("-----------------------");

			simulator = new Simulator();
			simulator.Start();

			Console.ReadKey();
		}
	}
}
