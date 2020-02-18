using RPGCore.Behaviour;
using RPGCore.Demo.BoardGame.Models;
using RPGCore.Traits;
using System.Collections.Generic;

namespace RPGCore.Demo.BoardGame
{
	public class GamePlayer
	{
		public LocalId OwnerId;
		public StatInstance CurrentScore;
		public BuildingTemplate SpecialCard;
		public GameBoard Board;

		public List<string> ResourceHand;
	}
}
