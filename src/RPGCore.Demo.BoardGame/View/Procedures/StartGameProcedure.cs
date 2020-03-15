using System.Collections.Generic;

namespace RPGCore.Demo.BoardGame
{
	public class StartGameProcedure : GameViewProcedure
	{
		public string[] Buildings { get; set; }
		public List<GamePlayer> Players { get; set; }

		public override ProcedureResult Apply(GameView view)
		{
			view.Buildings = Buildings;

			return ProcedureResult.Success;
		}
	}
}
