namespace RPGCore.Demo.BoardGame
{
	public class EndTurnProcedure : GameViewProcedure 
	{
		public override ProcedureResult Apply(GameView view)
		{
			view.CurrentPlayersTurn++;
			view.DeclaredResource = false;

			return ProcedureResult.Success;
		}
	}
}
