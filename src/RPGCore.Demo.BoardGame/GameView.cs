using RPGCore.Behaviour;
using RPGCore.Demo.BoardGame.Models;
using RPGCore.Packages;
using RPGCore.Traits;
using System.Collections.Generic;
using System.Linq;

namespace RPGCore.Demo.BoardGame
{
	public class GameView
	{
		public int CurrentPlayersTurn;
		public bool DeclaredResource;

		public BuildingTemplate[] Buildings;
		public GamePlayer[] Players;

		public IPackageExplorer GameData { get; private set; }

		public void Create(IPackageExplorer gameData)
		{
			GameData = gameData;

			Players = new GamePlayer[]
			{
				new GamePlayer()
				{
					CurrentScore = new StatInstance(),
					SpecialCard = new BuildingTemplate(),
					Board = new GameBoard(4, 4),
					ResourceHand = new List<string>()
				},
				new GamePlayer()
				{
					CurrentScore = new StatInstance(),
					SpecialCard = new BuildingTemplate(),
					Board = new GameBoard(4, 4),
					ResourceHand = new List<string>()
				}
			};
		}

		public void Apply(GameViewAction action)
		{
			action.Apply(this);
		}

		public GamePlayer GetPlayerForOwner(LocalId owner)
		{
			return Players.Where(player => player.OwnerId == owner).FirstOrDefault();
		}
	}
}
