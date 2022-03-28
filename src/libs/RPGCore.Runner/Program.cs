using System;
using System.IO;

namespace RPGCore.Runner;

internal sealed class Program
{
	private static void Main(string[] args)
	{
		Console.ForegroundColor = ConsoleColor.Gray;
		Console.WriteLine("-----------------------");
		Console.Write(" RPGCore ");
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.Write("2.0");
		Console.ForegroundColor = ConsoleColor.Gray;
		Console.Write(" Framework\n");
		Console.WriteLine("-----------------------");

		var directory = new DirectoryInfo("Content");
		if (!directory.Exists)
		{
			directory = new DirectoryInfo("RPGCoreUnity/Content");
		}
		Directory.SetCurrentDirectory(directory.Parent.FullName);

		Console.ReadLine();
	}
}
