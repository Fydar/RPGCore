using NUnit.Framework;
using RPGCore.Packages;

namespace RPGCore.Demo.BoardGame.UnitTests
{
	public class GameViewShould
	{
		[Test]
		public void CreateSuccessfully()
		{
			var explorer = new PackageExplorer();

			var gameView = new GameView();

			gameView.Create(explorer);
		}
	}
}
