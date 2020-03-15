using RPGCore.Behaviour;
using RPGCore.Traits;
using System.Collections.Generic;

namespace RPGCore.Demo.BoardGame
{
	public class PlayerJoinedProcedure : GameViewProcedure
	{
		public string DisplayName { get; set; }
		public LocalId OwnerId { get; set; }

		public override ProcedureResult Apply(GameView view)
		{
			view.Players.Add(new GamePlayer()
			{
				OwnerId = OwnerId,
				DisplayName = DisplayName,
				Board = new GameBoard(),
				CurrentScore = new StatInstance(),
				ResourceHand = new List<string>(),
				SpecialCards = null
			});

			return ProcedureResult.Success;
		}
	}
}
