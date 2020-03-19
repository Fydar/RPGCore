using RPGCore.Behaviour;
using RPGCore.Traits;
using System.Collections.Generic;

namespace RPGCore.Demo.BoardGame
{
	public class GamePlayerState
	{
		public LocalId OwnerId { get; set; }

		public StatInstance CurrentScore { get; set; }
		public List<BoardCardSlot> Buildings { get; set; }
		public List<SpecialCardSlot> SpecialCards { get; set; }
		public GameBoard Board { get; set; }
		public List<string> ResourceHand { get; set; }
	}
}
