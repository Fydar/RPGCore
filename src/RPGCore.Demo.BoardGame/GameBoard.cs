namespace RPGCore.Demo.BoardGame
{
	public class GameBoard
	{
		public GameTile[,] Tiles;

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
	}
}
