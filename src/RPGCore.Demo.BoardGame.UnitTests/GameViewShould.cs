using NUnit.Framework;
using RPGCore.Behaviour;
using RPGCore.Packages;
using System;

namespace RPGCore.Demo.BoardGame.UnitTests
{
	[TestFixture(TestOf = typeof(GameView))]
	public class GameViewShould
	{
		[Test, Parallelizable]
		public void CreateSuccessfully()
		{
			Assert.Ignore();

			var player1 = LocalId.NewShortId();
			var player2 = LocalId.NewShortId();

			PackageExplorer explorer = null;
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
			gameView.Apply(new PlaceResourceAction()
			{
				Client = player2,
				ResourceIdentifier = "1",
				ResourcePosition = new Integer2(3, 3)
			});
			gameView.Apply(new EndTurnAction()
			{
				Client = player1
			});

			gameView.Apply(new DeclareResourceAction()
			{
				Client = player2,
				ResourceIdentifier = "2"
			});
			gameView.Apply(new PlaceResourceAction()
			{
				Client = player1,
				ResourceIdentifier = "2",
				ResourcePosition = new Integer2(2, 1)
			});
			gameView.Apply(new PlaceResourceAction()
			{
				Client = player2,
				ResourceIdentifier = "2",
				ResourcePosition = new Integer2(2, 1)
			});
			gameView.Apply(new EndTurnAction()
			{
				Client = player2
			});

			gameView.Apply(new DeclareResourceAction()
			{
				Client = player1,
				ResourceIdentifier = "3"
			});
			gameView.Apply(new PlaceResourceAction()
			{
				Client = player1,
				ResourceIdentifier = "3",
				ResourcePosition = new Integer2(1, 2)
			});
			gameView.Apply(new PlaceResourceAction()
			{
				Client = player2,
				ResourceIdentifier = "3",
				ResourcePosition = new Integer2(1, 2)
			});

			DrawGameState(gameView);

			gameView.Apply(new BuildBuildingAction()
			{
				Client = player1,
				BuildingIdentifier = "building",
				BuildingPosition = new Integer2(2, 2),
				Offset = new Integer2(1, 1),
				Orientation = BuildingOrientation.None
			});
			gameView.Apply(new EndTurnAction()
			{
				Client = player1
			});

			DrawGameState(gameView);
		}

		private void DrawGameState(GameView game)
		{
			for (int i = 0; i < game.Players.Length; i++)
			{
				var player = game.Players[i];
				Console.WriteLine($"Player {player.OwnerId}");
				if (game.CurrentPlayersTurn == i)
				{
					Console.WriteLine($"(Current players turn)");
				}

				for (int y = 4 - 1; y >= 0; y--)
				{
					for (int x = 0; x < player.Board.Width; x++)
					{
						var tile = player.Board[x, y];

						Console.Write(tile.ToChar());
					}
					Console.WriteLine();
				}
				Console.WriteLine();
			}
			Console.WriteLine("=======");
		}
	}
}
