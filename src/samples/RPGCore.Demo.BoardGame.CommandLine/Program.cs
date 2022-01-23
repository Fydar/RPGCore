using System.Threading.Tasks;

namespace RPGCore.Demo.BoardGame.CommandLine;

internal sealed class Program
{
	private static async Task Main(string[] args)
	{
		var gameRunner = new GameRunner();

		await gameRunner.StartAsync();
	}
}
