using RPGCore.Behaviour;
using RPGCore.Traits;
using System.Collections.Generic;

namespace RPGCore.Demo.BoardGame
{

	public class GamePlayer
	{
		public LocalId OwnerId;
		public StatInstance CurrentScore;
		public SpecialCardSlot SpecialCard;
		public GameBoard Board;

		public List<string> ResourceHand;
	}
}
