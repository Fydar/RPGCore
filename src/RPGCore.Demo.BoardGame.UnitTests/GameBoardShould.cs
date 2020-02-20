using NUnit.Framework;

namespace RPGCore.Demo.BoardGame.UnitTests
{
	[TestFixture(TestOf = typeof(GameBoard))]
	public class GameBoardShould
	{
		[Test(Description = "Ensures that buildings are rotated properly."), Parallelizable]
		public void RotateBuildings()
		{
			var board = new GameBoard();

			
		}
	}
}
