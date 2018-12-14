using System;

namespace Runner
{
	class Program
	{
		public static Simulator simulator;

		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");

			simulator = new Simulator();
			simulator.Start();

			Console.ReadKey();
		}
	}
}
