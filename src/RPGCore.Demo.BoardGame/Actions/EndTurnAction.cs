namespace RPGCore.Demo.BoardGame
{
	public class EndTurnAction : GameViewAction
	{
		public override ActionApplyResult Apply(GameView view)
		{
			view.CurrentPlayersTurn++;
			view.DeclaredResource = false;

			return ActionApplyResult.Success;
		}
	}
}
