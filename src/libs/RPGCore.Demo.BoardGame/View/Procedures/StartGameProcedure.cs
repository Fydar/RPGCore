namespace RPGCore.Demo.BoardGame;

public class StartGameProcedure : LobbyViewProcedure
{
	public GameplayView Gameplay { get; set; }

	public override ProcedureResult Apply(LobbyView view)
	{
		view.Gameplay = Gameplay;

		return ProcedureResult.Success;
	}
}
