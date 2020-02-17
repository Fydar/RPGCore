namespace RPGCore.Demo.BoardGame
{
	public class GameBoard
	{
		public GameTile[,] Tiles;

		public GameBoard(int width = 4, int height = 4)
		{
			Tiles = new GameTile[width, height];

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					Tiles[x, y] = new GameTile();
				}
			}
		}
	}
}
