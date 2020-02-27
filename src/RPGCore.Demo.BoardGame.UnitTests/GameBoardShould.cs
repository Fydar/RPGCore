using NUnit.Framework;
using RPGCore.Demo.BoardGame.Models;
using System;

namespace RPGCore.Demo.BoardGame.UnitTests
{
	[TestFixture(TestOf = typeof(GameBoard))]
	public class GameBoardShould
	{
		[Test(Description = "Should be able to determine ever location a building can be built in."), Parallelizable]
		public void DetermineAllBuildableLocations()
		{
			var board = new GameBoard(4, 4);

			board[0, 0].Resource = "x";
			board[0, 1].Resource = "x";

			board[2, 1].Resource = "x";
			board[3, 1].Resource = "x";

			board[0, 2].Resource = "y";
			board[0, 3].Resource = "y";
			board[1, 2].Resource = "y";
			board[1, 3].Resource = "y";

			for (int y = 4 - 1; y >= 0; y--)
			{
				for (int x = 0; x < board.Width; x++)
				{
					var tile = board[x, y];

					Console.Write(tile.ToChar());
				}
				Console.Write("\n");
			}
			Console.Write("\n");

			var cottage = new BuildingTemplate()
			{
				Recipe = new string[,]
				{
					{ "x", "x" }
				}
			};

			var blockCottage = new BuildingTemplate()
			{
				Recipe = new string[,]
				{
					{ "y", "y" },
					{ "y", null }
				}
			};

			var longCottage = new BuildingTemplate()
			{
				Recipe = new string[,]
				{
					{ "x", "x", "x" },
					{ "x", null, null }
				}
			};

			Console.WriteLine($"--[Cottage]--");
			foreach (var location in board.AllBuildableLocations(cottage))
			{
				Console.WriteLine($"{location.Offset} {location.Orientation}");
			}

			Console.WriteLine($"\n--[Block Cottage]--");
			foreach (var location in board.AllBuildableLocations(blockCottage))
			{
				Console.WriteLine($"{location.Offset} {location.Orientation}");
			}

			Console.WriteLine($"\n--[Block Cottage]-- (minus bottom)");
			foreach (var location in board.AllBuildableLocations(blockCottage, new IntegerRect(0, 2, 4, 2)))
			{
				Console.WriteLine($"{location.Offset} {location.Orientation}");
			}
		}
	}
}
