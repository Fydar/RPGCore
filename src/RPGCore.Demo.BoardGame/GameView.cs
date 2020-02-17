using RPGCore.Demo.BoardGame.Models;
using RPGCore.Packages;
using RPGCore.Traits;

namespace RPGCore.Demo.BoardGame
{
	public class GameView
	{
		public string DisplayName;

		public BuildingTemplate[] Buildings;
		public GamePlayer[] Players;

		public void Create(IPackageExplorer gameData)
		{
			Players = new GamePlayer[]
			{
				new  GamePlayer()
				{
					CurrentScore = new StatInstance(),
					SpecialCard = new BuildingTemplate(),
					Board = new GameBoard(4, 4)
				},
				new  GamePlayer()
				{
					CurrentScore = new StatInstance(),
					SpecialCard = new BuildingTemplate(),
					Board = new GameBoard(4, 4)
				}
			};
		}
	}
}
