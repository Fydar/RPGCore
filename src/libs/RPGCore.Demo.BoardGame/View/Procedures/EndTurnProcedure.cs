using RPGCore.Behaviour;

namespace RPGCore.Demo.BoardGame
{
	public class EndTurnProcedure : LobbyViewProcedure
	{
		public LocalId Player { get; set; }

		public override ProcedureResult Apply(LobbyView view)
		{
			view.Gameplay.CurrentPlayersTurn++;
			view.Gameplay.DeclaredResource = false;

			return ProcedureResult.Success;
		}
	}
}
