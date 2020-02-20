using NUnit.Framework;
using RPGCore.Behaviour;
using RPGCore.Packages;

namespace RPGCore.Demo.BoardGame.UnitTests
{
	public class GameViewShould
	{
		[Test, Parallelizable]
		public void CreateSuccessfully()
		{
			var player1 = LocalId.NewId();
			var player2 = LocalId.NewId();

			var explorer = new PackageExplorer();
			var gameView = new GameView();
			gameView.Create(explorer);
			gameView.Players = new GamePlayer[]
			{
				new GamePlayer()
				{
					OwnerId = player1,
					Board = new GameBoard(4, 4),
				},
				new GamePlayer()
				{
					OwnerId = player2,
					Board = new GameBoard(4, 4),
				}
			};

			gameView.Apply(new DeclareResourceAction()
			{
				Client = player1,
				ResourceIdentifier = "1"
			});

			gameView.Apply(new PlaceResourceAction()
			{
				Client = player1,
				ResourceIdentifier = "1",
				ResourcePosition = new Integer2(1, 1)
			});

			gameView.Apply(new EndTurnAction()
			{
				Client = player1
			});

		}
	}
}
