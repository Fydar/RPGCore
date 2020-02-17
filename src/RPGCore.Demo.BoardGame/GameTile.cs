namespace RPGCore.Demo.BoardGame
{
	public class GameTile
	{
		public Building Building;
		public string Resource;

		public GameBoard Board { get; }

		public GameTile(GameBoard board)
		{
			Board = board;
		}
	}
}
