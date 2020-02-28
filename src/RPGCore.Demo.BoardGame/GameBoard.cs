using RPGCore.Demo.BoardGame.Models;
using System.Collections.Generic;

namespace RPGCore.Demo.BoardGame
{
	public class GameBoard
	{
		public GameTile[,] Tiles;

		public int Width
		{
			get
			{
				return Tiles.GetLength(0);
			}
		}

		public int Height
		{
			get
			{
				return Tiles.GetLength(1);
			}
		}

		public GameTile this[int x, int y]
		{
			get
			{
				return this[new Integer2(x, y)];
			}
		}

		public GameTile this[Integer2 position]
		{
			get
			{
				if (position.x < 0 || position.y < 0)
				{
					return null;
				}

				int width = Tiles.GetLength(0);
				int height = Tiles.GetLength(1);

				if (position.x >= width || position.y >= height)
				{
					return null;
				}

				return Tiles[position.x, position.y];
			}
		}

		public GameBoard(int width = 4, int height = 4)
		{
			Tiles = new GameTile[width, height];

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					Tiles[x, y] = new GameTile(this);
				}
			}
		}

		public GameTile GetTileRelative(GameTile source, Integer2 relativePosition)
		{
			var tilePosition = GetTilePositon(source);

			var targetPosition = tilePosition + relativePosition;

			return targetPosition.HasValue
				? this[targetPosition.Value]
				: null;
		}

		public Integer2? GetTilePositon(GameTile tile)
		{
			int width = Tiles.GetLength(0);
			int height = Tiles.GetLength(1);

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					var otherTile = Tiles[x, y];

					if (otherTile == tile)
					{
						return new Integer2(x, y);
					}
				}
			}
			return null;
		}

		public IEnumerable<OffsetAndRotation> AllBuildableLocations(BuildingTemplate buildingTemplate, IntegerRect? rect = null)
		{
			int boardWidth = Width;
			int boardHeight = Height;

			var boundingRect = rect ?? new IntegerRect(0, 0, boardWidth, boardHeight);

			foreach (var orientation in buildingTemplate.MeaningfulOrientations())
			{
				var rotatedBuilding = new RotatedBuilding(buildingTemplate, orientation);

				int availableXOffset = boundingRect.xMax - rotatedBuilding.Width + 1;
				int availableYOffset = boundingRect.yMax - rotatedBuilding.Height + 1;

				for (int buildingOffsetX = boundingRect.x; buildingOffsetX < availableXOffset; buildingOffsetX++)
				{
					for (int buildingOffsetY = boundingRect.y; buildingOffsetY < availableYOffset; buildingOffsetY++)
					{
						var buildLocation = new OffsetAndRotation(new Integer2(buildingOffsetX, buildingOffsetY), orientation);

						if (CanBuildBuilding(buildLocation, rotatedBuilding))
						{
							yield return buildLocation;
						}
					}
				}
			}
		}

		public bool CanBuildBuilding(OffsetAndRotation offsetAndRotation, RotatedBuilding rotatedBuilding)
		{
			for (int x = 0; x < rotatedBuilding.Width; x++)
			{
				for (int y = 0; y < rotatedBuilding.Height; y++)
				{
					var tilePos = offsetAndRotation.Offset + new Integer2(x, y);
					var tile = this[tilePos];

					if (tile == null)
					{
						return false;
					}

					string resourceAtPos = rotatedBuilding[x, y];

					if (resourceAtPos == null)
					{
						continue;
					}

					if (tile.Resource != resourceAtPos)
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
