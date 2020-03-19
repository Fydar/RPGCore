using NUnit.Framework;
using RPGCore.Behaviour;
using RPGCore.Packages;
using System;
using System.Collections.Generic;

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
			gameView.StartGame(explorer);

			gameView.Apply(new PlayerJoinedProcedure()
			{
				OwnerId = player1,
				DisplayName = "Player 1"
			});

			gameView.Apply(new PlayerJoinedProcedure()
			{
				OwnerId = player2,
				DisplayName = "Player 2"
			});

			gameView.Apply(new StartGameProcedure()
			{
				Buildings = new string[]
				{
					"cottagepack"
				},
				PlayerStates = gameView.PlayerStates
			});

			gameView.Apply(new DeclareResourceProcedure()
			{
				Player = player1,
				ResourceIdentifier = "1"
			});
			gameView.Apply(new PlaceResourceProcedure()
			{
				Player = player1,
				ResourceIdentifier = "1",
				ResourcePosition = new Integer2(1, 1)
			});
			gameView.Apply(new PlaceResourceProcedure()
			{
				Player = player2,
				ResourceIdentifier = "1",
				ResourcePosition = new Integer2(3, 3)
			});
			gameView.Apply(new EndTurnProcedure()
			{
				Player = player1
			});

			gameView.Apply(new DeclareResourceProcedure()
			{
				Player = player2,
				ResourceIdentifier = "2"
			});
			gameView.Apply(new PlaceResourceProcedure()
			{
				Player = player1,
				ResourceIdentifier = "2",
				ResourcePosition = new Integer2(2, 1)
			});
			gameView.Apply(new PlaceResourceProcedure()
			{
				Player = player2,
				ResourceIdentifier = "2",
				ResourcePosition = new Integer2(2, 1)
			});
			gameView.Apply(new EndTurnProcedure()
			{
				Player = player2
			});

			gameView.Apply(new DeclareResourceProcedure()
			{
				Player = player1,
				ResourceIdentifier = "3"
			});
			gameView.Apply(new PlaceResourceProcedure()
			{
				Player = player1,
				ResourceIdentifier = "3",
				ResourcePosition = new Integer2(1, 2)
			});
			gameView.Apply(new PlaceResourceProcedure()
			{
				Player = player2,
				ResourceIdentifier = "3",
				ResourcePosition = new Integer2(1, 2)
			});

			DrawGameState(gameView);

			gameView.Apply(new BuildBuildingProcedure()
			{
				Player = player1,
				BuildingIdentifier = "building",
				BuildingPosition = new Integer2(2, 2),
				Offset = new Integer2(1, 1),
				Orientation = BuildingOrientation.None
			});
			gameView.Apply(new EndTurnProcedure()
			{
				Player = player1
			});

			DrawGameState(gameView);
		}

		private void DrawGameState(GameView game)
		{
			for (int i = 0; i < game.PlayerStates.Count; i++)
			{
				var playerStates = game.PlayerStates[i];
				Console.WriteLine($"Player {playerStates.OwnerId}");
				if (game.CurrentPlayersTurn == i)
				{
					Console.WriteLine($"(Current players turn)");
				}

				for (int y = 4 - 1; y >= 0; y--)
				{
					for (int x = 0; x < playerStates.Board.Width; x++)
					{
						var tile = playerStates.Board[x, y];

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
