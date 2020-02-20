using NUnit.Framework;
using RPGCore.Packages;

namespace RPGCore.Demo.BoardGame.UnitTests
{
	public class GameViewShould
	{
		[Test, Parallelizable]
		public void CreateSuccessfully()
		{
			var explorer = new PackageExplorer();

			var gameView = new GameView();

			gameView.Create(explorer);
		}
	}
}
