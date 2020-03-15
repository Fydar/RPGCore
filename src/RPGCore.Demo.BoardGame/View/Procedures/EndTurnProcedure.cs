using RPGCore.Behaviour;

namespace RPGCore.Demo.BoardGame
{
	public class EndTurnProcedure : GameViewProcedure
	{
		public LocalId Player { get; set; }

		public override ProcedureResult Apply(GameView view)
		{
			view.CurrentPlayersTurn++;
			view.DeclaredResource = false;

			return ProcedureResult.Success;
		}
	}
}
