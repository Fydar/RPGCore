using RPGCore.Behaviour;
using RPGCore.Traits;
using System.Collections.Generic;

namespace RPGCore.Demo.BoardGame
{
	public class GamePlayer
	{
		public string DisplayName { get; set; }
		public LocalId OwnerId { get; set; }

		public StatInstance CurrentScore { get; set; }
		public List<SpecialCardSlot> SpecialCards { get; set; }
		public GameBoard Board { get; set; }
		public List<string> ResourceHand { get; set; }
	}
}
