namespace RPGCore.Demo.BoardGame.CommandLine
{
	internal sealed class Program
	{
		private static void Main(string[] args)
		{
			var gameRunner = new GameRunner();

			gameRunner.Start();
		}
	}
}
