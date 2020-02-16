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

		}
	}

	public class GamePlayer
	{
		public StatInstance CurrentScore;

		public BuildingTemplate SpecialCard;
	}
}
